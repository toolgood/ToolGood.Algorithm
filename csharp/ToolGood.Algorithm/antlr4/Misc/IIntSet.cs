/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System.Collections.Generic;
namespace Antlr4.Runtime.Misc
{
    public interface IIntSet
    {
        void Add(int el);
        IIntSet AddAll(IIntSet set);
        IIntSet And(IIntSet a);
        IIntSet Complement(IIntSet elements);
        IIntSet Or(IIntSet a);
        IIntSet Subtract(IIntSet a);
        int Count
        {
            get;
        }
        bool IsNil
        {
            get;
        }
        bool Equals(object obj);
        int SingleElement
        {
            get;
        }
        bool Contains(int el);
        void Remove(int el);
        IList<int> ToList();
    }
}
