# Anonymous test data with AutoFixture

Often times we have unit or integration tests that rely on some input data. The easiest solution is just to take some hard-coded values and move on with life. This has some major downsides: 

Giving specific values in a test carries meaning, but we are often times not interested in that. We just need to pass the object around to fulfill the API. Also, the simplest solution to fulfill your test is literally checking against those values. 

Here is an elegant solution to that problem: `AutoFixture`. I will show you what it can do, especially in combination with `xUnit`.

Found [here](https://steven-giesel.com/blogPost/0a7d1bff-6c97-4d98-97cf-d72cd2b3888e)