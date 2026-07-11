using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.RequestHelper
{
    public class Pagination<T>(int PageIndex , int Pagesize , int count , IReadOnlyList<T> data )
    {
        public int PageIndex { get; set; } = PageIndex;
       public int PageSize { get; set; } = Pagesize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> data { get; set; } = data;


    }
}
