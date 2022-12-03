# How to enumerate through a StringBuilder

Did you ever wonder how we can iterate through a `StringBuilder`? I mean, of course, we can just call `ToString` and use the returned string, but that means we materialize the whole thing without good reason. 

We can also use a normal for-loop. But we can also find a completely different and probably dumber way!  And if you wonder: No, this is not something you do in your daily life, but by doing that thing, I can show some cool stuff C# and .NET offer.

Found [here](https://steven-giesel.com/blogPost/5161909a-a223-4312-b939-996a0f97bf75)