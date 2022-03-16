using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace TodoApp
{
    public class NewTodoItemViewModel
    {
        private string title;
        private string description;

        public string Title
        {
            get => title; set
            {
                title = value;                
            }
        }

        public string Description
        {
            get => description; set
            {
                description = value;                
            }
        }

        public DateTimeOffset DueDate { get; set; }

    }
}
