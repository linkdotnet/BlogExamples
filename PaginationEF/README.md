# Easy Pagination for Entity Framework in 3 steps

Pagination is the process of dividing a set into discrete pages. In the context of Entity Framework, that means we are only getting a certain amount of entries from the database.

And we will implement a very easy solution to make that happen in 3 steps. The result will look like this:
```csharp
var pagedList = DbContext.BlogPosts.ToPagedList(page: 1, pageSize: 5);
```

Found [here](https://steven-giesel.com/blogPost/09285b33-79e6-4879-95e0-35aeae5fbcc6)