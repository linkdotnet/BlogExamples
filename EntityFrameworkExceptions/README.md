# Gracefully Handling Entity Framework Exceptions with EntityFramework.Exceptions

Working with databases can sometimes be daunting, mainly when errors occur. These errors or exceptions can be due to many reasons, such as constraint violations, connection issues, or syntax errors. Entity Framework throws a generic DbException or DbUpdateException for most of these database issues. But we cand get more specific exceptions based on the concrete "problem"! That's where EntityFramework.Exceptions comes in.

Found [here](https://steven-giesel.com/blogPost/693e8b9c-4b97-43a4-8bf7-991c633900b4)