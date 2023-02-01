using CRD.Enums;
using Newtonsoft.Json;

namespace CRD.Models
{
    public class GenericResponseWithoutData : GenericResponse<object>
    {
        public GenericResponseWithoutData(StatusCode status) : base(status)
        {

        }
    }

    public class GenericResponse<T>
    {
        [JsonProperty(PropertyName = "status")]
        public StatusCode Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        [JsonProperty(PropertyName = "response")]
        public T Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
            }
        }


        private StatusCode status;
        private T response;

        public GenericResponse(StatusCode status)
        {
            this.status = status;
        }
        public GenericResponse(StatusCode status, T response)
        {
            this.status = status;
            this.response = response;
        }
    }
}

