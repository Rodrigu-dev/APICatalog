using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Pagination
{
    public class QueryStringParameters
    {
        const int maxPageSize = 50;
        const int maxPageNumber = 20;
        private int _pageSize = 10;
        private int _pageNumber = 1;
        
        public int PageNumber {
            get
            {
                return _pageNumber;
            }
            set 
            {
                _pageNumber = (value > maxPageNumber) ? maxPageNumber : value;
            } 
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
