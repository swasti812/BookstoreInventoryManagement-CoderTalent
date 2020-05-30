using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class BookOperationsModel
    {
        public BookTable DetailsUpdate(BookTable Book, BookTable BookObj)
        {
            Book.ISBN = BookObj.ISBN;
            Book.Publisher = BookObj.Publisher;
            Book.Author = BookObj.Author;
            Book.Title = BookObj.Title;
            Book.Year = BookObj.Year;
            return Book;
        }

    }
}