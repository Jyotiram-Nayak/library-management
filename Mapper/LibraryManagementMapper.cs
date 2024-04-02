﻿using AutoMapper;
using library_management.Data.Model;
using library_management.Data.ViewModel;

namespace library_management.Mapper
{
    public class LibraryManagementMapper : Profile
    {
        public LibraryManagementMapper()
        {
            CreateMap<Books, BooksVM>();
            CreateMap<BooksVM, Books>();
            CreateMap<Authors, AuthorsVM>();
            CreateMap<AuthorsVM, Authors>();
            CreateMap<BorrowBookVM, Borrowings>();
            CreateMap<BookISBNVM, BooksISBN>();
        }
    }
}
