using System;
using System.Collections;

namespace V2.Helpdesk.web
{
    /// <summary>
    /// Summary description for IssueStatus.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Return the Sorted List
        /// </summary>
        /// <param name="EnumType"> Pass the Enum Name </param>
        /// <param name="InitialValue">Initial Value can be blank also</param>
        /// <returns></returns>

        public static SortedList BindStatusEnum(Type EnumType, string InitialValue)
        {
            // get the names from the enumeration
            string[] names = Enum.GetNames(EnumType);
            // get the values from the enumeration
            Array values = Enum.GetValues(EnumType);
            // turn it into a hash table
            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();
            SortedList st = new SortedList();
            if (InitialValue != "")
                st.Add(InitialValue, "-1");

            for (int i = 0; i < names.Length; i++)
                // note the cast to integer here is important
                // otherwise we'll just get the enum string back again
                st.Add(names[i], (int)values.GetValue(i));
            // return the dictionary to be bound to

            return st;
        }
    }

    public enum IssueStatus : int { New = 1, Resolved = 2, Moved = 3, Reopen = 4 };
}