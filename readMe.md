Rectangles and Segments Intersection

Summary:

I created a simple .Net5 Web App with simple Api controller, simple service and interface, and a business logic class.
For simplicity I used InMemoryStorage which I did via using a list of rectangles. Why list? Because I dont know number of elements, and list allowas me to add endless number of elements.
Also I wrote 2 unit tests, where tested service and business logic class. Besides that I imlemented Result pattern for error handling.

After all I Pushed it to Github.

To test the api you have to checkout the project, launch it locally. For simplicity I added swagger, so you will see api.

Test Api:

1. GetIntersectingRectangles
   To send request to GetIntersectingRectangles method of Api, you can go to this link.
   api/Rectangle/findIntersectingRectangles
