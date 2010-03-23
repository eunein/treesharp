﻿#region License

//     A simplistic Behavior Tree implementation in C#
//     Copyright (C) 2010  ApocDev apocdev@gmail.com
// 
//     This file is part of TreeSharp.
// 
//     TreeSharp is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     TreeSharp is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System.Collections.Generic;

namespace TreeSharp
{
    /// <summary>
    ///   The base sequence class. This will execute each branch of logic, in order.
    ///   If all branches succeed, this composite will return a successful run status.
    ///   If any branch fails, this composite will return a failed run status.
    /// </summary>
    public class Sequence : GroupComposite
    {
        public Sequence(params Composite[] children) : base(children)
        {
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            foreach (Composite node in Children)
            {
                node.Start(context);
                while (node.Tick(context) == RunStatus.Running)
                {
                    Selection = node;
                    yield return RunStatus.Running;
                }

                Selection = null;
                node.Stop(context);

                if (node.LastStatus == RunStatus.Failure)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }
                yield return RunStatus.Running;
            }
            yield return RunStatus.Success;
        }
    }
}