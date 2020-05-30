using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    
    public class InventoryController : ApiController
    {
        
        [HttpGet]
        [Route("api/Inventory/Insert")]
      public HttpResponseMessage InsertBook([FromBody] BookTable NewObj)
        {
            try
            {
                var context = new InventoryManagementEntities();
                context.BookTable.Add(NewObj);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Book Added");

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Inventory/ShowBooks")]
        public HttpResponseMessage ShowBooks()
        {
            try
            {
               var context = new InventoryManagementEntities();
               var ReturnList= context.BookTable.Select(p=> new {p.ISBN,p.Publisher,p.Title,p.Year,p.Author }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, ReturnList);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Inventory/ModifyDetails")]
        public HttpResponseMessage ModifyDetails([FromBody] int BookISBN)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var Book = context.BookTable.SingleOrDefault(p=> p.ISBN==BookISBN);
                if (Book == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Not Found, Check ISBN");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, Book);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Inventory/SaveChanges")]
        public HttpResponseMessage SaveChanges([FromBody] BookTable BookObj)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var Book = context.BookTable.SingleOrDefault(p=>p.ISBN == BookObj.ISBN);
                //Book.ISBN = BookObj.ISBN;
                //Book.Publisher = BookObj.Publisher;
                //Book.Author = BookObj.Author;
                //Book.Title = BookObj.Title;
                //Book.Year = BookObj.Year;
                Book = new BookOperationsModel().DetailsUpdate(Book, BookObj);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Changes Saved");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
