namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq.Expressions;

    public static class Guard 
    {
        public static CheckData<TData> For<TData>(Expression<Func<TData>> data)
        {
            var member = data.Body as MemberExpression;
            var name = member != null 
                        ? member.Member.Name 
                        : "anonymous value";
            
            return For(data.Compile().Invoke(), name);
        }

        public static CheckData<TData> For<TData>(TData data, string name)
        {
            return new CheckData<TData>
                {
                    ValueName = name,
                    Value = data
                };
        }
    }
}
