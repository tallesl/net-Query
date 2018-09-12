namespace QueryLibrary
{
    using System.Globalization;

    internal class Join
    {
        internal JoinType Type { get; set; }

        internal string Table { get; set; }

        internal Condition On { get; set; }

        private string JoinString
        {
            get
            {
                switch (Type)
                {
                    case JoinType.Inner:
                        return "INNER JOIN";

                    case JoinType.LeftOuterJoin:
                        return "LEFT OUTER JOIN";

                    case JoinType.RightOuterJoin:
                        return "RIGHT OUTER JOIN";

                    case JoinType.FullOuterJoin:
                        return "FULL OUTER JOIN";

                    case JoinType.CrossJoin:
                        return "CROSS JOIN";

                    default:
                        return "JOIN";
                }
            }
        }

        public override string ToString()
        {
            return On == null ?
                string.Format(CultureInfo.InvariantCulture, "{0} {1}", JoinString, Table) :
                string.Format(CultureInfo.InvariantCulture, "{0} {1} ON {2}", JoinString, Table, On);
        }
    }
}