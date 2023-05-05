# RESTful Web API - The Complete Guide (.NET7 API)

## Build RESTful web API (C#) with Authentication and learn how to consume them in a real world application (.NET 7 API)

### Description

- This is a Beginner to Intermediate level course on ASP.NET Core Web API that will take you from the basics of building API to consuming them. This course is for anyone who is new to RESTful Web API's in ASP.NET Core or who is familiar with ASP.NET and wants to learn how to consume them effectively in an ASP.NET Core Web application.

- By the end of this course, you will be able to build a RESTful web service with Web API by yourself, make GET, POST, PUT and DELETE HTTP Requests with a well-built repository pattern in ASP.NET Core Project. You will also get a exposure to Entity Framework Code First migrations and learn how to save your data persistently in a database.

- We will cover authentication and authorization in Web API as well as consume them in a real-world project.

- Finally the complete project will be deployed to azure!

#### What am I going to get from this course?

1. Learn basic fundamentals of ASP NET Core web API
2. Build RESTful API's in .NET 7
3. Learn how to document an API using swagger and swashbuckle.
4. Versioning in an API.
5. Implement Repository Pattern in API to the database using EF.
6. Authentication and Authorization in ASP.NET Core API's.
7. Integrate Entity Framework along with code first migrations
8. Learn how to consume API using HTTPClient in the Repository Pattern.
9. Deploying .NET 7 API

- All source codes and exercise solutions of this course are also available on Github and you can find details in the lecture "PROJECT RESOURCES", of course.

### Requirements

- 6months knowlege of C#
- 3-6 months knowledge of NET Core
- Visual Studio 2022
- SQL Server 2018
- .NET 7

---

Thanks [Bhrugen Patel](https://www.dotnetmastery.com/)

---

## Note

1. Add services to the container.

> AddSingleton: Dịch vụ được đăng ký với phạm vi AddSingleton sẽ được tạo ra một lần duy nhất trong suốt vòng đời ứng dụng. Điều này có nghĩa là, nếu bạn sử dụng AddSingleton để đăng ký một dịch vụ, một thể hiện của dịch vụ đó sẽ được tạo ra khi ứng dụng bắt đầu chạy và sẽ được sử dụng trong suốt vòng đời ứng dụng.
> AddSingleton thường được sử dụng cho các dịch vụ chung và có tính chất toàn cục, chẳng hạn như các dịch vụ cấu hình, các dịch vụ lưu trữ bộ đệm chung, các dịch vụ quản lý phiên, v.v.

> AddScoped: Dịch vụ được đăng ký với phạm vi AddScoped sẽ được tạo ra một lần duy nhất cho mỗi yêu cầu HTTP và được sử dụng lại trong toàn bộ quá trình xử lý yêu cầu đó. Điều này có nghĩa là, nếu bạn sử dụng AddScoped để đăng ký một dịch vụ, một thể hiện của dịch vụ đó sẽ được tạo ra cho mỗi yêu cầu HTTP và sẽ được sử dụng lại trong toàn bộ quá trình xử lý yêu cầu đó. Sau khi quá trình xử lý yêu cầu kết thúc, thể hiện của dịch vụ đó sẽ bị giải phóng.
> AddScoped thường được sử dụng cho các dịch vụ liên quan đến xử lý yêu cầu HTTP, chẳng hạn như các dịch vụ liên quan đến truy vấn cơ sở dữ liệu, các dịch vụ lưu trữ bộ đệm, các dịch vụ chứng thực, v.v.

> AddTransient: Dịch vụ được đăng ký với phạm vi AddTransient sẽ được tạo ra mỗi khi có yêu cầu truy cập dịch vụ. Điều này có nghĩa là, nếu bạn sử dụng AddTransient để đăng ký một dịch vụ, một thể hiện của dịch vụ đó sẽ được tạo ra mỗi khi có yêu cầu truy cập
> AddTransient thường được sử dụng cho các dịch vụ nhỏ, nhẹ và không tốn nhiều tài nguyên, chẳng hạn như các dịch vụ truy xuất dữ liệu không liên quan đến cơ sở dữ liệu, các dịch vụ lưu trữ bộ đệm riêng tư, các dịch vụ khởi tạo các đối tượng cục bộ, v.v.
