namespace QueryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A condition with optional AND and OR clauses that can be used in WHERE or JOIN ON statements.
    /// </summary>
    public class Condition
    {
        private readonly List<string> _tokens = new List<string>();

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="condition">Condition to set in this instance</param>
        public Condition(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            _tokens.Add(condition);
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="condition">Copies to the condition being constructed</param>
        public Condition(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            _tokens.Add(condition.ParenthesisToString());
        }

        /// <summary>
        /// Appends the given condition with AND in this condition.
        /// </summary>
        /// <param name="condition">Condition to be appended</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Condition And(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            _tokens.Add("AND");
            _tokens.Add(condition);
            return this;
        }

        /// <summary>
        /// Appends the given condition with AND in this condition.
        /// </summary>
        /// <param name="condition">Condition to be appended</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Condition And(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            return And(condition.ParenthesisToString());
        }

        /// <summary>
        /// Appends the given condition with OR in this condition.
        /// </summary>
        /// <param name="condition">Condition to be appended</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Condition Or(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            _tokens.Add("OR");
            _tokens.Add(condition);
            return this;
        }

        /// <summary>
        /// Appends the given condition with OR in this condition.
        /// </summary>
        /// <param name="condition">Condition to be appended</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Condition Or(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            return Or(condition.ParenthesisToString());
        }

        /// <summary>
        /// Returns the condition statement as a SQL query in parenthesis.
        /// </summary>
        /// <returns>The condition statement as a SQL query in parenthesis</returns>
        public string ParenthesisToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0})", ToString());
        }

        /// <summary>
        /// Returns the condition statement as a SQL query.
        /// </summary>
        /// <returns>The condition statement as a SQL query</returns>
        public override string ToString()
        {
            return string.Join(" ", _tokens);
        }
    }
}
