#region Copyright (c) 2009-2014 Misakai Ltd.
/*************************************************************************
* 
* This file is part of Spike.Build Project.
*
* Spike.Build is free software: you can redistribute it and/or modify it 
* under the terms of the GNU General Public License as published by the 
* Free Software Foundation, either version 3 of the License, or (at your
* option) any later version.
*
* Foobar is distributed in the hope that it will be useful, but WITHOUT 
* ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
* or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public 
* License for more details.
* 
* You should have received a copy of the GNU General Public License 
* along with Foobar. If not, see http://www.gnu.org/licenses/.
*************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spike.Build
{
    public partial class Element
    {
        /// <summary>
        /// Type to be used by the generated library (inside)
        /// </summary>
        public string InternalType { get; set; }

        /// <summary>
        /// Name to be used by the generated library (inside)
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Name to be used by the generated library (inside)
        /// </summary>
        public string InternalElementType { get; set; }

        /// <summary>
        /// Gets whether the element is a complex type or a list of complex types
        /// </summary>
        public bool IsComplexType
        {
            get { return Type == ElementType.ComplexType || Type == ElementType.ListOfComplexType; }
        }

        /// <summary>
        /// Gets whether the element is a dynamic type or a list of dynamic types
        /// </summary>
        public bool IsDynamicType
        {
            get { return Type == ElementType.DynamicType || Type == ElementType.ListOfDynamicType; }
        }

        /// <summary>
        /// Gets whether the element is a simple type
        /// </summary>
        public bool IsSimpleType
        {
            get { return !IsComplexType; }
        }

        /// <summary>
        /// Gets whether the element is a list
        /// </summary>
        public bool IsList
        {
            get { return Type.ToString().StartsWith("ListOf"); }
        }

        /// <summary>
        /// Gets server-side element type
        /// </summary>
        public string ServerElementType
        {
            get { return Type.ToString().Replace("ListOf", ""); }
        }


        /// <summary>
        /// Gets the list of members
        /// </summary>
        public List<Element> GetMembers()
        {
            return this.Member;
        }

        /// <summary>
        /// Gets the list of members recursively
        /// </summary>
        public List<Element> GetAllMembers()
        {
            return GetAllMembers(false);
        }

        /// <summary>
        /// Gets the list of members recursively
        /// </summary>
        public List<Element> GetAllMembers(bool addSelf)
        {
            var result = new List<Element>();
            if (this.IsComplexType)
            {
                if (addSelf)
                    result.Add(this);
                if (this.Member.Count > 0)
                {
                    foreach (var element in Member)
                        result.AddRange(element.GetAllMembers(true));
                }
            }
            else
            {
                result.Add(this);
            }
            return result;
        }


        #region Overrides
        public override string ToString()
        {
            return String.Format("Element: {0} of type {1}", Name, Type);
        }

        public override bool Equals(object obj)
        {
            var right = obj as Element;
            if (right == null)
                return false;
            if (right.Type == this.Type && this.IsComplexType && right.Class == this.Class && right.Name == this.Name)
                return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (this.IsComplexType)
                return Class.GetHashCode();
            return base.GetHashCode();
        }
        #endregion
    }
}
