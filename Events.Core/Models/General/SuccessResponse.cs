
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Core.Models.General
{
    public class SuccessResponse<T> : ControllerBase
    {
        
        public long id { get; set; }
        public T instance { get; set; }

        public List<T> items { get; set; }

        public int status { get; set; } = (int)ResponseCodes.SuccessCode;

        public bool isSuccessResponseObject { get; set; } = true;

        public bool isChangeAction { get; set; } = false;


        public static object build(T it, long i = 0, List<T> list = null)
        => new { status = ResponseCodes.SuccessCode, data = new SuccessResponse<T>(it, i, list) };

        private SuccessResponse(T it , long i = 0, List<T> list = null) {
            instance = it;
            items = list;
            id = i;
        }
    }
}
