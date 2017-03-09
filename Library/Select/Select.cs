namespace QueryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Class that aids building a SELECT clause.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
        Justification = "Harder for consumers in other languages but easy to remember for consumers of this language.")]
    public class Select
    {
        internal List<string> _columns;

        internal string _from;

        internal List<Join> _joins;

        internal Condition _where;

        internal List<string> _groupBy;

        internal string _having;

        internal List<string> _orderBy;

        internal bool _desc;

        /// <summary>
        /// Class that aids building a SELECT clause.
        /// </summary>
        /// <param name="columns">Columns to be selected</param>
        public Select(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (_columns == null)
                _columns = new List<string>(columns);
            else
                _columns.AddRange(columns);
        }

        /// <summary>
        /// Sets the FROM clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be selected from</param>
        /// <returns></returns>
        public Select From(string table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (_from != null)
                throw new SelectBuildingException("FROM clause already set.");

            _from = table;
            return this;
        }

        /// <summary>
        /// Sets a JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select Join(string table, string on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.None, table, new Condition(on));
        }

        /// <summary>
        /// Sets a JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select Join(string table, Condition on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.None, table, on);
        }

        /// <summary>
        /// Sets a LEFT OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select LeftOuterJoin(string table, string on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.LeftOuterJoin, table, new Condition(on));
        }

        /// <summary>
        /// Sets a LEFT OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select LeftOuterJoin(string table, Condition on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.LeftOuterJoin, table, on);
        }

        /// <summary>
        /// Sets a RIGHT OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select RightOuterJoin(string table, string on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.RightOuterJoin, table, new Condition(on));
        }

        /// <summary>
        /// Sets a RIGHT OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select RightOuterJoin(string table, Condition on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.RightOuterJoin, table, on);
        }

        /// <summary>
        /// Sets a FULL OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select FullOuterJoin(string table, string on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.FullOuterJoin, table, new Condition(on));
        }

        /// <summary>
        /// Sets a FULL OUTER JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <param name="on">Condition of the join (ON clause)</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select FullOuterJoin(string table, Condition on)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (on == null)
                throw new ArgumentNullException("on");

            return _Join(JoinType.FullOuterJoin, table, on);
        }

        /// <summary>
        /// Sets a CROSS JOIN clause in the SELECT being built.
        /// </summary>
        /// <param name="table">Table to be join</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select CrossJoin(string table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            return _Join(JoinType.CrossJoin, table, null);
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select Where(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where != null)
                throw new SelectBuildingException("WHERE clause already set.");

            _where = new Condition(condition);
            return this;
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// Throws exception if WHERE is already set.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        /// <exception cref="SelectBuildingException">WHERE clause is already set</exception>
        public Select Where(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where != null)
                throw new SelectBuildingException("WHERE clause is already set.");

            _where = new Condition(condition);
            return this;
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// If WHERE is already set, appends the condition with an AND clause.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select WhereAnd(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where == null)
                return Where(condition);

            _where.And(condition);
            return this;
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// If WHERE is already set, appends the condition with an AND clause.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select WhereAnd(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where == null)
                return Where(condition);

            _where.And(condition);
            return this;
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// If WHERE is already set, appends the condition with an OR clause.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select WhereOr(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where == null)
                return Where(condition);

            _where.Or(condition);
            return this;
        }

        /// <summary>
        /// Sets the WHERE clause in the SELECT being built.
        /// If WHERE is already set, appends the condition with an OR clause.
        /// </summary>
        /// <param name="condition">Condition of the WHERE clause</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select WhereOr(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_where == null)
                return Where(condition);

            _where.Or(condition);
            return this;
        }

        /// <summary>
        /// Sets the GROUP BY clause in the SELECT being built.
        /// </summary>
        /// <param name="columns">Columns to be grouped by</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select GroupBy(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (_groupBy == null)
                _groupBy = new List<string>(columns);
            else
                _groupBy.AddRange(columns);

            return this;
        }

        /// <summary>
        /// Sets the HAVING clause in the SELECT being built.
        /// Throws exception if HAVING is already set.
        /// </summary>
        /// <param name="condition">Condition to set</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        /// <exception cref="SelectBuildingException">HAVING clause is already set</exception>
        public Select Having(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (_having != null)
                throw new SelectBuildingException("HAVING clause is already set.");

            _having = condition;
            return this;
        }

        /// <summary>
        /// Sets the ORDER BY clause in the SELECT being built.
        /// </summary>
        /// <param name="columns">Columns to be ordered by</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        public Select OrderBy(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (_orderBy == null)
                _orderBy = new List<string>(columns);
            else
                _orderBy.AddRange(columns);

            _desc = false;

            return this;
        }

        /// <summary>
        /// Sets the ORDER BY clause in the SELECT being built with DESC.
        /// </summary>
        /// <param name="columns">Columns to be ordered by</param>
        /// <returns>This instance, so you can use it in a fluent fashion</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
            Justification = "It's not hungarian notation, it's just short for 'descending'.")]
        public Select OrderByDesc(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (_orderBy == null)
                _orderBy = new List<string>(columns);
            else
                _orderBy.AddRange(columns);

            _desc = true;

            return this;
        }

        /// <summary>
        /// Operator overload that allows using the class wherever a string is expected.
        /// </summary>
        public static implicit operator string(Select select)
        {
            return select == null ? null : select.ToString();
        }

        /// <summary>
        /// Returns the SELECT statement as a SQL query in parenthesis (subselect).
        /// </summary>
        /// <returns>The SELECT statement as a SQL query in parenthesis</returns>
        public string ParenthesisToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0})", ToString());
        }

        /// <summary>
        /// Returns the SELECT statement as a SQL query.
        /// </summary>
        /// <returns>The SELECT statement as a SQL query</returns>
        public override string ToString()
        {
            var sql = new StringBuilder("SELECT ");
            sql.Append(string.Join(", ", _columns));

            if (_from != null)
            {
                sql.Append(" FROM ");
                sql.Append(_from);
            }

            if (_joins != null)
            {
                foreach (var join in _joins)
                    sql.Append(string.Format(CultureInfo.InvariantCulture, " {0}", join));
            }

            if (_where != null)
            {
                sql.Append(" WHERE ");
                sql.Append(_where);
            }

            if (_groupBy != null)
            {
                sql.Append(" GROUP BY ");
                sql.Append(string.Join(", ", _groupBy));
            }

            if (_having != null)
            {
                sql.Append(" HAVING ");
                sql.Append(_having);
            }

            if (_orderBy != null)
            {
                sql.Append(" ORDER BY ");
                sql.Append(string.Join(", ", _orderBy));

                if (_desc)
                    sql.Append(" DESC");
            }

            return sql.ToString();
        }

        private Select _Join(JoinType type, string table, Condition on)
        {
            var join = new Join { Type = type, Table = table, On = on, };

            if (_joins == null)
                _joins = new List<Join> { join };
            else
                _joins.Add(join);

            return this;
        }
    }
}
