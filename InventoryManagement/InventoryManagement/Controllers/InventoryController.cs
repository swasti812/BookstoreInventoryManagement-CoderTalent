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
     //Central controller for all major actions   
        [HttpPost]
        [Route("api/Inventory/Insert")]
      public HttpResponseMessage InsertBook([FromBody] BookTable NewObj)
        { 
            //Method to Insert Book
            try
            {
                var context = new InventoryManagementEntities();
                //Checking if given book already exists by using unique ISBN number
                var BookExist = context.BookTable.SingleOrDefault(p => p.ISBN == NewObj.ISBN);
                if (BookExist == null)
                {
                    context.BookTable.Add(NewObj);
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Book Added");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Ambiguous, "Book Already Exists in Database");
                }

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Inventory/ShowBooks")]

        //Method to display list of all books in Inventory
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
        //Takes ISBN Number to show details of book which has to be modified
        public HttpResponseMessage ModifyDetails([FromBody] long BookISBN)
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
                var Book = context.BookTable.SingleOrDefault(p => p.ISBN == BookObj.ISBN);
                Book = new BookOperationsModel().DetailsUpdate(Book, BookObj);
                context.SaveChanges();
                //Save New cahnges in Book Details
                return Request.CreateResponse(HttpStatusCode.OK, "Changes Saved");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpDelete]
        [Route("api/Inventory/DeleteBook")]
        //Method to Delete Book from Inventory
        public HttpResponseMessage DeleteBook([FromBody] BookTable book)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var Book = context.BookTable.SingleOrDefault(p => p.ISBN == book.ISBN);
                context.BookTable.Remove(Book);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Changes Saved");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Methods to search books by Author ,ISBN or Title

        [HttpGet]
        [Route("api/Inventory/SearchAuthor")]
        public HttpResponseMessage SearchAuthor([FromBody] string SearchVal)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var result = context.BookTable.Where(x => x.Author.Contains(SearchVal)).ToList();
                //Returns list of all Books with Authors equal to or containing search string
                if(result==null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No results Found");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpGet]
        [Route("api/Inventory/SearchISBN")]
        public HttpResponseMessage SearchISBN([FromBody] long SearchVal)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var result = context.BookTable.Where(x => x.ISBN==SearchVal).ToList();
                //Returns list of all Books with ISBN equal to or containing search string
                if (result == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No results Found");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [HttpGet]
        [Route("api/Inventory/SearchTitle")]
        public HttpResponseMessage SearchTitle([FromBody] string SearchVal)
        {
            try
            {
                var context = new InventoryManagementEntities();
                var result = context.BookTable.Where(x => x.Title.Contains(SearchVal)).ToList();
                //Returns list of all Books with Title equal to or containing search string
                if (result == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No results Found");
                else
                    return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
