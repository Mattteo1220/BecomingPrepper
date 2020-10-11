## BecomingPrepper
  Is a Web Application with an Api Service that will gradually aid ordinary citizens preparedness for emergency situations and doomsday type events, ultimately, BecomingPrepper. To aid in both the short and long term of emergency preparedness. BecomingPrepper will allow citizens to stock up on every day items such as food, water, first Aid, Ammo and track their products. Preppers will be able to Update, Add, and Delete products from their inventory as well as track their progress against a selected objective created during account registration. These objectives range from two weeks to twenty years. As their inventory grows, so too will their confidence in being prepared for the unknown. 

  Not only is this a great tracking tool for inventory needs but it is also a way to learn to live off the environment around them. There are ten categories for which preppers should stock up. Some of them are Water, Grains, Sugars, Canned Fruits and Vegetables and Dried Meats. With each product, a cooresponding image will be returned from the Mongo Database. These images are designed in a way to not only give the prepper a physical sense of what each product is, but teaches the prepper about where in the environment they can encounter these products. i.e. olive oil is natural to temperate climates such as the Mediterranean as well as areas in South America and Australia. The image returned would be able to educate the prepper about where to find the product, seasons to harvest, preparation and long term preservation.  

### Technologies
###### BackEnd
- C#
- ASP.NET Core
- Gherkin
- Mongo DB
- Grid Fs
- Specflow
- Fluent Assertions
- Fixture
- Xunit
- JSON Web Token or JWT (Api Authorization)

###### FrontEnd
- Yet To come

### Design And Analysis
  In order for the project to be understood and designed, I had to undergo an analysis process utilizing UML or Unified Modeling Language. This helped me design the API portion of BecomingPrepper. 
  Here below are the links to the videos as I demonstrate the analysis and design phase of this project. 
 - [First Design Session](https://www.youtube.com/watch?v=MgYgdPEFuso)
 - [Second Design Session](https://www.youtube.com/watch?v=FTVtyh6OuhI)
 
 ### Tenants
  Within the project, I needed to follow some software practices and guidelines as I underwent the development or enhancements portions of this project. Some of these tenants within software revolved around, security, testing, databases, or advanced software. I chose to prove three of these tenants which are:
  - Software Security
  - Software Quality And Testing
  - Database Management through Mongo DB
  
Through these tenants I learned quite a bit.
- Security : I learned how to implement JWT or Json Web Tokens to secure my api endpoints. Through this I can ensure that my endpoint's are locked down and that only through true validation on the login screen can users actually utilize the service. I also followed the practice of defensible programming as I ensure that if invalid, null or empty parameters or arguments are caught and exceptions are thrown and messages returned to ensure that the application doesn't crash unexpectedly.

- Software Quality and Testing: Since this is my forte and current position, this was a little easier to implement but I undertook a different approach. I learned new technologies and libraries to expand my skill set. I implemented Behavior Driven Development utilizing specflow with c# and fluent assertions with Fixture for unit tests to create smaller, more manageable test suites. Below is an example of Fluent Assertions with Fixture.
[FluentAssertionsExample](https://github.com/Mattteo1220/BecomingPrepper/blob/master/BecomingPrepper.Tests/UnitTests/Api/FoodStorage/GetInventoryItemShould.cs)

- Database Management through Mongo Db: This was a fun way to learn a new database management system. Mongo DB is a no sql document based management system. Through collections, documents instead of databases and tables, I am able to prove that Preppers can update, add, delete and read their data effectively and securely on this web application. GridFS is a specification for storing and retrieving large files such as documents, images, and videos. GridFS utilizes two collections, one that stores file details and the other that stores the bytes of the file itself called chunks.
