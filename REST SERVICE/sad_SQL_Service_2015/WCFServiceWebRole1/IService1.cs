using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    [ServiceContract]
    public interface IService1
    {
        // defines the uri template for calling the addToList method
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
        UriTemplate = "lists?guid={guid}&userid={userid}&title={title}&task={task}")]
        void addToList(Guid guid, string userid, string title, string task);

        // defines the uri template for calling the viewListJSON method
        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "viewlist?format=JSON&guid={guid}&userid={userid}")]
        List_Item[] viewListJSON(string guid, string userid);

    }
    [DataContract]

    //defines the structure of the JSON object
    public class List_Item
    {
        [DataMember]
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        private string guid;
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        [DataMember]
        private string userid;
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        [DataMember]
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [DataMember]
        private string task;
        public string Task
        {
            get { return task; }
            set { task = value; }
        }
    }
}