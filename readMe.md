Rectangles and Segments Intersection

Summary:

I created a simple .Net8.0 Web App with simple Api controller, simple service and interface, and a business logic class.
For simplicity I used InMemoryStorage which I did via using a list of rectangles. Why list? Because I dont know number of elements, and list allowas me to add endless number of elements.
Also I wrote 2 unit tests, where tested service and business logic class. Besides that I imlemented Result pattern for error handling.

After all I Pushed it to Github.

To make setup easier for you I added a dockerfile and compose file. So when you checkout repo, you need to execute:

docker compose up

To test the api you have to open postman and send this request:

curl --location 'http://localhost:8080/findIntersectingRectangles' \
--header 'Content-Type: application/json' \
--data '[
  {
    "x": 0,
    "y": 3
  },
  {
    "x": 3,
    "y": 3
    }
]'


You should get 1 rectangle object as response, because in inmemorystorage i added 2 rectangles, one of them will be intersecting.
