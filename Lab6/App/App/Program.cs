using App.Entities;
using App.Services.DataBaseService;
using App.Services.DataService;
using System;
using System.Collections.Generic;

namespace App
{
    class Program
    {
        static IDataBaseService dbService = new DataBaseService("Host=localhost;Port=5432;Database=MDiSUBD_Astromarket;Username=postgres;Password=12345;");
        static IDataService dataService = new DataService(dbService);
        static User currentUser;
        static Cart usersCart;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the AstroMarket!");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void Login()
        {
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            currentUser = dataService.GetUserByEmail(email);

            if (currentUser != null && currentUser.Password == password)
            {
                Console.WriteLine("Login successful!");
                UserMenu();
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
                Console.ReadKey();
            }
        }

        static void Register()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            
            int Role_id = 2;

            User newUser = new User { Name = name, Email = email, Password = password, Role_id = Role_id };

            dataService.CreateUser(newUser);
            
            newUser = dataService.GetUserByEmail(email);
            Cart cart = new Cart();
            cart.Date = DateTime.Now;
            cart.Total_Price = 0;
            cart.User_Id = newUser.Id;
            dataService.CreateCart(cart);
            Console.WriteLine("Registration successful! Now you can Log In!");
           
            
            Console.ReadKey();
        }

        static void UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome {currentUser.Name}!");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. View Orders");
                Console.WriteLine("3. View Cart");
                Console.WriteLine("4. Update Account");
                Console.WriteLine("5. Sort Products by Category");
                Console.WriteLine("6. Manage Cart");
                Console.WriteLine("7. Leave a Review");
                Console.WriteLine("8. Watch product reviews");
                Console.WriteLine("9. Make order");
                Console.WriteLine("10. Logout");

                if (currentUser.Role_id == 1) 
                {
                    Console.WriteLine("11. Admin Menu");
                }

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        ViewOrders();
                        break;
                    case "3":
                        ViewCart();
                        break;
                    case "4":
                        UpdateAccount();
                        break;
                    case "5":
                        SortProductsByCategory();
                        break;
                    case "6":
                        ManageCart();
                        break;
                    case "7":
                        LeaveReview();
                        break;
                    case "8":
                        WatchReviews();
                        break;
                    case "9":
                        MakeOrder();
                        break;
                    case "10":
                        return;
                    case "11":
                        if (currentUser.Role_id == 1)
                            AdminMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Manage Products");
                Console.WriteLine("2. Manage Categories");
                Console.WriteLine("3. Manage Payment Methods");
                Console.WriteLine("4. Manage Delivery Methods");
                Console.WriteLine("5. Manage Roles");
                Console.WriteLine("6. View Action Journal");
                Console.WriteLine("7. View Journal by User");
                Console.WriteLine("8. Back to User Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageProducts();
                        break;
                    case "2":
                        ManageCategories();
                        break;
                    case "3":
                        ManagePaymentMethods();
                        break;
                    case "4":
                        ManageDeliveryMethods();
                        break;
                    case "5":
                        ManageRoles();
                        break;
                    case "6":
                        ViewActionJournal();
                        break;
                    case "7":
                        ViewJournalsByUser();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void ViewProducts()
        {
            var products = dataService.GetProducts(null);
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price:C}");
            }
            Console.ReadKey();
        }

        static void ViewOrders()
        {
            var orders = dataService.GetOrdersByUserId(currentUser.Id);
            foreach (var order in orders)
            {
                Console.WriteLine($"{order.Id}: {order.Status} - {order.Total_Price:C}");
            }
            Console.ReadKey();
        }

        static void ViewCart()
        {
            usersCart = dataService.GetCartByUserId(currentUser.Id);
            var cartProducts = dataService.GetProductsFromCart(usersCart.Id);
            foreach (var item in cartProducts)
            {
                Console.WriteLine($"{item.Product_Name} - {item.Quantity} - {item.Total_Price}");
            }
            Console.ReadKey();
        }

        static void UpdateAccount()
        {
            Console.Write("Enter New Name (leave blank to keep current): ");
            string name = Console.ReadLine();
            Console.Write("Enter New Password (leave blank to keep current): ");
            string password = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
                currentUser.Name = name;
            if (!string.IsNullOrEmpty(password))
                currentUser.Password = password;

            dataService.UpdateUser(currentUser);
            Console.WriteLine("Account updated successfully!");
            Console.ReadKey();
        }

        static void SortProductsByCategory()
        {
            Console.Write("Enter Category ID: ");
            int Category = int.Parse(Console.ReadLine());
            var products = dataService.GetProducts(Category);
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price:C}");
            }
            Console.ReadKey();
        }

        static void ManageCart()
        {
            usersCart = dataService.GetCartByUserId(currentUser.Id);
            List<CartItem> cartItems = dataService.GetProductsFromCart(usersCart.Id);
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Manage Cart (Add/Remove/Update items)");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. Remove Item");
                Console.WriteLine("3. Update Item Quantity");
                Console.WriteLine("4. View Cart");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                Product prod;
                bool success;
                switch (choice)
                {
                    case "1":
                        // Add Item
                        Console.Write("Enter product ID to add: ");
                        int addProductId = int.Parse(Console.ReadLine());
                        Console.Write("Enter quantity: ");
                        int addQuantity = int.Parse(Console.ReadLine());

                        prod = dataService.GetProductById(addProductId);
                        if (prod != null)
                        {
                            dataService.AddToCart(usersCart, prod, addQuantity);
                            Console.WriteLine("Item added to cart.");
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                        break;

                    case "2":
                        Console.Write("Enter product ID to remove: ");
                        int removeProductId = int.Parse(Console.ReadLine());
                        prod = dataService.GetProductById(removeProductId);
                        success = dataService.DeleteFromCart(usersCart, prod);

                        if (success)
                        {
                            Console.WriteLine("Item removed from cart.");
                        }
                        else
                        {
                            Console.WriteLine("Item not found in cart.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter product ID to update: ");
                        int updateProductId = int.Parse(Console.ReadLine());
                        Console.Write("Enter new quantity: ");
                        int newQuantity = int.Parse(Console.ReadLine());
                        prod = dataService.GetProductById(updateProductId);
                        success = dataService.ChangeProductQuantity(usersCart, prod, newQuantity);
                    
                        if (success)
                        {
                            Console.WriteLine("Item quantity updated.");
                        }
                        else
                        {
                            Console.WriteLine("Item not found in cart.");
                        }
                        break;

                    case "4":
                        cartItems = dataService.GetProductsFromCart(usersCart.Id);
                        Console.WriteLine("Your Cart Items:");
                        foreach (var item in cartItems)
                        {
                            Console.WriteLine($"Product ID: {item.Id}, Name: {item.Product_Name}, Quantity: {item.Quantity}, Total Price: {item.Total_Price:C}");
                        }
                        Console.ReadKey();
                        break;

                    case "5":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void LeaveReview()
        {
            Console.Write("Enter Product ID: ");
            int Product_Id = int.Parse(Console.ReadLine());
            Console.Write("Enter Review Text: ");
            string text = Console.ReadLine();
            Console.Write("Enter Rating (1-5): ");
            int rating = int.Parse(Console.ReadLine());

            Review review = new Review
            {
                User_Id = currentUser.Id,
                Product_Id = Product_Id,
                Text = text,
                Rating = rating
            };

            dataService.AddReview(review);
            Console.WriteLine("Review added successfully!");
            Console.ReadKey();
        }

        static void WatchReviews()
        {
            Console.Write("Enter Product ID: ");
            int Product_Id = int.Parse(Console.ReadLine());
            var reviews = dataService.GetReviews(Product_Id);
            foreach (var review in reviews)
            {
                Console.WriteLine($"Text: {review.Text} Rating: {review.Rating}");
            }
            Console.ReadKey();
        }

        static void MakeOrder()
        {
            var paymentMethods = dataService.GetPaymentMethods();
            var deliveryMethods = dataService.GetDeliveryMethods();

            Console.WriteLine("Select a payment method:");
            for (int i = 0; i < paymentMethods.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {paymentMethods[i].Name}");
            }

            int selectedPaymentIndex = int.Parse(Console.ReadLine());

            Console.WriteLine("Select a delivery method:");
            for (int i = 0; i < deliveryMethods.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {deliveryMethods[i].Name}");
            }

            int selectedDeliveryIndex = int.Parse(Console.ReadLine());

            usersCart = dataService.GetCartByUserId(currentUser.Id);
            DateTime date = DateTime.Now;
            var order = new Order
            {
                User_Id = currentUser.Id,
                Date = date,
                Status = "Pending", 
                Total_Price = usersCart.Total_Price, 
                Payment_Method = selectedPaymentIndex, 
                Delivery_Method = selectedDeliveryIndex 
            };

            dataService.AddOrder(order);

            order = dataService.GetOrder(currentUser.Id, date);

            var cartItems = dataService.GetProductsFromCart(usersCart.Id);
            foreach (var item in cartItems)
            {
                dataService.AddProductOrder(item.Id, order.Id, item.Total_Price);
            }

            dataService.ClearCart(usersCart.Id);

            Console.WriteLine("Order created successfully!");
            Console.ReadKey();
        }
        static void ManageProducts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Products:");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. View All Products");
                Console.WriteLine("5. Back to Admin Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        UpdateProduct();
                        break;
                    case "3":
                        DeleteProduct();
                        break;
                    case "4":
                        ViewAllProducts();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddProduct()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Product Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Product Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Product newProduct = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = categoryId
            };

            if (dataService.AddProduct(newProduct))
            {
                Console.WriteLine("Product added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add product.");
            }
            Console.ReadKey();
        }

        static void UpdateProduct()
        {
            Console.Write("Enter Product ID to Update: ");
            int productId = int.Parse(Console.ReadLine());
            Product product = dataService.GetProductById(productId);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter New Name (leave blank to keep current): ");
            string name = Console.ReadLine();
            Console.Write("Enter New Description (leave blank to keep current): ");
            string description = Console.ReadLine();
            Console.Write("Enter New Price (leave blank to keep current): ");
            string priceInput = Console.ReadLine();
            Console.Write("Enter New Category ID (leave blank to keep current): ");
            string categoryInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(name)) product.Name = name;
            if (!string.IsNullOrEmpty(description)) product.Description = description;
            if (!string.IsNullOrEmpty(priceInput)) product.Price = decimal.Parse(priceInput);
            if (!string.IsNullOrEmpty(categoryInput)) product.Category = int.Parse(categoryInput);

            if (dataService.UpdateProduct(product))
            {
                Console.WriteLine("Product updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update product.");
            }
            Console.ReadKey();
        }

        static void DeleteProduct()
        {
            Console.Write("Enter Product ID to Delete: ");
            int productId = int.Parse(Console.ReadLine());
            Product product = dataService.GetProductById(productId);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                Console.ReadKey();
                return;
            }

            if (dataService.DeleteProduct(product))
            {
                Console.WriteLine("Product deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete product.");
            }
            Console.ReadKey();
        }

        static void ViewAllProducts()
        {
            var products = dataService.GetProducts(null);
            Console.WriteLine("Products List:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price:C}");
            }
            Console.ReadKey();
        }

        static void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Categories:");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. Delete Category");
                Console.WriteLine("3. View All Categories");
                Console.WriteLine("4. Back to Admin Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCategory();
                        break;
                    case "2":
                        DeleteCategory();
                        break;
                    case "3":
                        ViewAllCategories();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddCategory()
        {
            Console.Write("Enter Category Name: ");
            string name = Console.ReadLine();

            Category newCategory = new Category { Name = name };

            if (dataService.AddCategory(newCategory))
            {
                Console.WriteLine("Category added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add category.");
            }
            Console.ReadKey();
        }

        static void DeleteCategory()
        {
            Console.Write("Enter Category ID to Delete: ");
            int categoryId = int.Parse(Console.ReadLine());
            Category category = dataService.GetCategories().FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                Console.ReadKey();
                return;
            }

            if (dataService.DeleteCategory(category))
            {
                Console.WriteLine("Category deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete category.");
            }
            Console.ReadKey();
        }

        static void ViewAllCategories()
        {
            var categories = dataService.GetCategories();
            Console.WriteLine("Categories List:");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }
            Console.ReadKey();
        }

        static void ManagePaymentMethods()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Payment Methods:");
                Console.WriteLine("1. Add Payment Method");
                Console.WriteLine("2. Delete Payment Method");
                Console.WriteLine("3. View All Payment Methods");
                Console.WriteLine("4. Back to Admin Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPaymentMethod();
                        break;
                    case "2":
                        DeletePaymentMethod();
                        break;
                    case "3":
                        ViewAllPaymentMethods();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddPaymentMethod()
        {
            Console.Write("Enter Payment Method Name: ");
            string name = Console.ReadLine();

            PaymentMethod newPaymentMethod = new PaymentMethod { Name = name };

            if (dataService.AddPaymentMethod(newPaymentMethod))
            {
                Console.WriteLine("Payment method added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add payment method.");
            }
            Console.ReadKey();
        }

        static void DeletePaymentMethod()
        {
            Console.Write("Enter Payment Method ID to Delete: ");
            int paymentMethodId = int.Parse(Console.ReadLine());
            PaymentMethod paymentMethod = dataService.GetPaymentMethods().FirstOrDefault(pm => pm.Id == paymentMethodId);

            if (paymentMethod == null)
            {
                Console.WriteLine("Payment method not found.");
                Console.ReadKey();
                return;
            }

            if (dataService.DeletePaymentMethod(paymentMethod))
            {
                Console.WriteLine("Payment method deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete payment method.");
            }
            Console.ReadKey();
        }

        static void ViewAllPaymentMethods()
        {
            var paymentMethods = dataService.GetPaymentMethods();
            Console.WriteLine("Payment Methods List:");
            foreach (var pm in paymentMethods)
            {
                Console.WriteLine($"{pm.Id}: {pm.Name}");
            }
            Console.ReadKey();
        }

        static void ManageDeliveryMethods()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Delivery Methods:");
                Console.WriteLine("1. Add Delivery Method");
                Console.WriteLine("2. Delete Delivery Method");
                Console.WriteLine("3. View All Delivery Methods");
                Console.WriteLine("4. Back to Admin Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddDeliveryMethod();
                        break;
                    case "2":
                        DeleteDeliveryMethod();
                        break;
                    case "3":
                        ViewAllDeliveryMethods();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddDeliveryMethod()
        {
            Console.Write("Enter Delivery Method Name: ");
            string name = Console.ReadLine();

            DeliveryMethod newDeliveryMethod = new DeliveryMethod { Name = name };

            if (dataService.AddDeliveryMethod(newDeliveryMethod))
            {
                Console.WriteLine("Delivery method added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add delivery method.");
            }
            Console.ReadKey();
        }

        static void DeleteDeliveryMethod()
        {
            Console.Write("Enter Delivery Method ID to Delete: ");
            int deliveryMethodId = int.Parse(Console.ReadLine());
            DeliveryMethod deliveryMethod = dataService.GetDeliveryMethods().FirstOrDefault(dm => dm.Id == deliveryMethodId);

            if (deliveryMethod == null)
            {
                Console.WriteLine("Delivery method not found.");
                Console.ReadKey();
                return;
            }

            if (dataService.DeleteDeliveryMethod(deliveryMethod))
            {
                Console.WriteLine("Delivery method deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete delivery method.");
            }
            Console.ReadKey();
        }

        static void ViewAllDeliveryMethods()
        {
            var deliveryMethods = dataService.GetDeliveryMethods();
            Console.WriteLine("Delivery Methods List:");
            foreach (var dm in deliveryMethods)
            {
                Console.WriteLine($"{dm.Id}: {dm.Name}");
            }
            Console.ReadKey();
        }

        static void ManageRoles()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Manage Roles:");
                Console.WriteLine("1. Add Role");
                Console.WriteLine("2. Delete Role");
                Console.WriteLine("3. View All Roles");
                Console.WriteLine("4. Back to Admin Menu");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRole();
                        break;
                    case "2":
                        DeleteRole();
                        break;
                    case "3":
                        ViewAllRoles();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void AddRole()
        {
            Console.Write("Enter Role Name: ");
            string name = Console.ReadLine();

            Role newRole = new Role { Name = name };

            if (dataService.AddRole(newRole))
            {
                Console.WriteLine("Role added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add role.");
            }
            Console.ReadKey();
        }

        static void DeleteRole()
        {
            Console.Write("Enter Role ID to Delete: ");
            int roleId = int.Parse(Console.ReadLine());
            Role role = dataService.GetRoles().FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                Console.WriteLine("Role not found.");
                Console.ReadKey();
                return;
            }

            if (dataService.DeleteRole(role))
            {
                Console.WriteLine("Role deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete role.");
            }
            Console.ReadKey();
        }

        static void ViewAllRoles()
        {
            var roles = dataService.GetRoles();
            Console.WriteLine("Roles List:");
            foreach (var role in roles)
            {
                Console.WriteLine($"{role.Id}: {role.Name}");
            }
            Console.ReadKey();
        }

        static void ViewActionJournal()
        {
            var journalEntries = dataService.GetJournals();
            Console.Clear();
            Console.WriteLine("Action Journal:");

            if (journalEntries.Count == 0)
            {
                Console.WriteLine("No journal entries found.");
            }
            else
            {
                foreach (var entry in journalEntries)
                {
                    Console.WriteLine($"[{entry.Date}] User ID: {entry.User_Id}, Action: {entry.Action}");
                }
            }

            Console.ReadKey();
        }

        static void ViewJournalsByUser()
        {
            Console.Write("Enter User ID to filter journal entries: ");
            int userId = int.Parse(Console.ReadLine());
            var journalEntries = dataService.GetJournalsByUserId(userId);
            Console.Clear();
            Console.WriteLine($"Action Journal for User ID: {userId}");

            if (journalEntries.Count == 0)
            {
                Console.WriteLine("No journal entries found for this user.");
            }
            else
            {
                foreach (var entry in journalEntries)
                {
                    Console.WriteLine($"[{entry.Date}] Action: {entry.Action}");
                }
            }

            Console.ReadKey();
        }
    }
}