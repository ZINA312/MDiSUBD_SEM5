using App.Entities;
using App.Services.DataBaseService;
using System.Collections.Generic;
using System.Linq;

namespace App.Services.DataService
{
    public class DataService : IDataService
    {
        private readonly IDataBaseService _db;

        public DataService(IDataBaseService db)
        {
            _db = db;
        }

        public bool CreateUser(User user)
        {
            var query = "public.add_user";
            _db.AddParameter("p_name", user.Name);
            _db.AddParameter("p_email", user.Email);
            _db.AddParameter("p_password", user.Password);
            _db.AddParameter("p_role_id", user.Role_id);
            return _db.ExecuteNonQuery(query, true) > 0;
        }

        public bool CreateCart(Cart cart)
        {
            var query = "INSERT INTO carts (user_id, date, total_price) VALUES (@UserId, @Date, @TotalPrice)";
            _db.AddParameter("UserId", cart.User_Id);
            _db.AddParameter("Date", cart.Date);
            _db.AddParameter("TotalPrice", cart.Total_Price);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool UpdateUser(User user)
        {
            var query = "UPDATE users SET name = @Name, email = @Email, password = @Password, role_id = @RoleId WHERE id = @Id";
            _db.AddParameter("Name", user.Name);
            _db.AddParameter("Email", user.Email);
            _db.AddParameter("Password", user.Password);
            _db.AddParameter("RoleId", user.Role_id);
            _db.AddParameter("Id", user.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteUser(User user)
        {
            var query = "DELETE FROM users WHERE id = @Id";
            _db.AddParameter("Id", user.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public List<CartItem> GetProductsFromCart(int cartId)
        {
            var query = "SELECT p.id AS id, p.name AS product_name, p.description AS product_description, cp.quantity, cp.total_price " +
                        "FROM cart_product cp " +
                        "JOIN products p ON cp.product_id = p.id " +
                        "WHERE cp.cart_id = @CartId";
            _db.AddParameter("CartId", cartId);
            return _db.ExecuteQuery<CartItem>(query).ToList();
        }

        public bool AddToCart(Cart cart, Product product, int quantity)
        {
            var query = "INSERT INTO cart_product (cart_id, product_id, quantity, total_price) VALUES (@CartId, @ProductId, @Quantity, @TotalPrice)";
            _db.AddParameter("CartId", cart.Id);
            _db.AddParameter("ProductId", product.Id);
            _db.AddParameter("Quantity", quantity);
            decimal totalPrice = product.Price * quantity;
            _db.AddParameter("TotalPrice", totalPrice);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteFromCart(Cart cart, Product product)
        {
            var query = "DELETE FROM cart_product WHERE cart_id = @CartId AND product_id = @ProductId";
            _db.AddParameter("CartId", cart.Id);
            _db.AddParameter("ProductId", product.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool ChangeProductQuantity(Cart cart, Product product, int newQuantity)
        {
            var query = "UPDATE cart_product SET quantity = @NewQuantity, total_price = @TotalPrice WHERE cart_id = @CartId AND product_id = @ProductId";
            _db.AddParameter("NewQuantity", newQuantity);

            _db.AddParameter("CartId", cart.Id);
            _db.AddParameter("ProductId", product.Id);
            decimal totalPrice = product.Price * newQuantity;
            _db.AddParameter("TotalPrice", totalPrice);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddCategory(Category category)
        {
            var query = "INSERT INTO categories (name) VALUES (@Name)";
            _db.AddParameter("Name", category.Name);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteCategory(Category category)
        {
            var query = "DELETE FROM categories WHERE id = @Id";
            _db.AddParameter("Id", category.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddReview(Review review)
        {
            var query = "INSERT INTO review (user_id, product_id, text, rating) VALUES (@UserId, @ProductId, @Text, @Rating)";
            _db.AddParameter("UserId", review.User_Id);
            _db.AddParameter("ProductId", review.Product_Id);
            _db.AddParameter("Text", review.Text);
            _db.AddParameter("Rating", review.Rating);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteReview(Review review)
        {
            var query = "DELETE FROM review WHERE id = @Id";
            _db.AddParameter("Id", review.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddOrder(Order order)
        {
            var query = "INSERT INTO orders (user_id, date, status, total_price, payment_method, delivery_method) VALUES (@UserId, @Date, @Status, @TotalPrice, @PaymentMethod, @DeliveryMethod)";
            _db.AddParameter("UserId", order.User_Id);
            _db.AddParameter("Date", order.Date);
            _db.AddParameter("Status", order.Status);
            _db.AddParameter("TotalPrice", order.Total_Price);
            _db.AddParameter("PaymentMethod", order.Payment_Method);
            _db.AddParameter("DeliveryMethod", order.Delivery_Method);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteOrder(Order order)
        {
            var query = "DELETE FROM orders WHERE id = @Id";
            _db.AddParameter("Id", order.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool UpdateOrder(Order order)
        {
            var query = "UPDATE orders SET status = @Status, total_price = @TotalPrice WHERE id = @Id";
            _db.AddParameter("Status", order.Status);
            _db.AddParameter("TotalPrice", order.Total_Price);
            _db.AddParameter("Id", order.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddPaymentMethod(PaymentMethod paymentMethod)
        {
            var query = "INSERT INTO payment_methods (name) VALUES (@Name)";
            _db.AddParameter("Name", paymentMethod.Name);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            var query = "DELETE FROM payment_methods WHERE id = @Id";
            _db.AddParameter("Id", paymentMethod.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            var query = "INSERT INTO delivery_methods (name) VALUES (@Name)";
            _db.AddParameter("Name", deliveryMethod.Name);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            var query = "DELETE FROM delivery_methods WHERE id = @Id";
            _db.AddParameter("Id", deliveryMethod.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddRole(Role role)
        {
            var query = "INSERT INTO roles (name) VALUES (@Name)";
            _db.AddParameter("Name", role.Name);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteRole(Role role)
        {
            var query = "DELETE FROM roles WHERE id = @Id";
            _db.AddParameter("Id", role.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool AddProduct(Product product)
        {
            decimal price = product.Price;
            var query = "INSERT INTO products (name, description, price, category) VALUES (@Name, @Description, @Price, @Category)";
            _db.AddParameter("Name", product.Name);
            _db.AddParameter("Description", product.Description);
            _db.AddParameter("Price", price);
            _db.AddParameter("Category", product.Category);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool DeleteProduct(Product product)
        {
            var query = "DELETE FROM products WHERE id = @Id";
            _db.AddParameter("Id", product.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public bool UpdateProduct(Product product)
        {
            decimal price = product.Price;
            var query = "UPDATE products SET name = @Name, description = @Description, price = @Price, category = @Category WHERE id = @Id";
            _db.AddParameter("Name", product.Name);
            _db.AddParameter("Description", product.Description);
            _db.AddParameter("Price", price);
            _db.AddParameter("Category", product.Category);
            _db.AddParameter("Id", product.Id);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public void ClearCart(int cartId)
        {
            var query = "DELETE FROM cart_product WHERE cart_id = @CartId";
            _db.AddParameter("CartId", cartId);
            _db.ExecuteNonQuery(query);

            query = "UPDATE carts SET total_price = 0 WHERE id = @CartId";
            _db.AddParameter("CartId", cartId);
            _db.ExecuteNonQuery(query);
        }

        public bool AddProductOrder(int product_id, int order_id, decimal total_price)
        {
            var query = "INSERT INTO product_order (product_id, order_id, total_price) VALUES (@ProductId, @OrderId, @TotalPrice)";
            _db.AddParameter("ProductId", product_id);
            _db.AddParameter("OrderId", order_id);
            _db.AddParameter("TotalPrice", total_price);
            return _db.ExecuteNonQuery(query) > 0;
        }

        public Cart GetCartByUserId(int userId)
        {
            var query = "SELECT * FROM carts WHERE user_id = @UserId";
            _db.AddParameter("UserId", userId);
            return _db.ExecuteQuery<Cart>(query).FirstOrDefault();
        }

        public Product GetProductById(int id)
        {
            var query = "SELECT * FROM products WHERE id = @Id";
            _db.AddParameter("Id", id);
            return _db.ExecuteQuery<Product>(query).FirstOrDefault();
        }

        public List<Product> GetProducts(int? category)
        {
            var query = category.HasValue
                ? "SELECT * FROM products WHERE category = @Category"
                : "SELECT * FROM products";
            if (category.HasValue)
                _db.AddParameter("Category", category.Value);
            return _db.ExecuteQuery<Product>(query).ToList();
        }

        public User GetUserById(int id)
        {
            var query = "SELECT * FROM users WHERE id = @Id";
            _db.AddParameter("Id", id);
            return _db.ExecuteQuery<User>(query).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            var query = "SELECT * FROM users WHERE email = @Email";
            _db.AddParameter("Email", email);
            return _db.ExecuteQuery<User>(query).FirstOrDefault();
        }

        public List<Category> GetCategories()
        {
            var query = "SELECT * FROM categories";
            return _db.ExecuteQuery<Category>(query).ToList();
        }

        public List<Review> GetReviews(int productId)
        {
            var query = "SELECT * FROM review WHERE product_id = @ProductId";
            _db.AddParameter("ProductId", productId);
            return _db.ExecuteQuery<Review>(query).ToList();
        }

        public Order GetOrder(int user_id, DateTime date)
        {
            var query = "SELECT * FROM orders WHERE user_id = @UserId AND date = @Date";
            _db.AddParameter("UserId", user_id)
                .AddParameter("Date", date);
            return _db.ExecuteQuery<Order>(query).FirstOrDefault();
        }

        public List<Order> GetOrders()
        {
            var query = "SELECT * FROM orders";
            return _db.ExecuteQuery<Order>(query).ToList();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            var query = "SELECT * FROM orders WHERE user_id = @UserId";
            _db.AddParameter("UserId", userId);
            return _db.ExecuteQuery<Order>(query).ToList();
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            var query = "SELECT * FROM payment_methods";
            return _db.ExecuteQuery<PaymentMethod>(query).ToList();
        }

        public List<DeliveryMethod> GetDeliveryMethods()
        {
            var query = "SELECT * FROM delivery_methods";
            return _db.ExecuteQuery<DeliveryMethod>(query).ToList();
        }

        public List<Journal> GetJournals()
        {
            var query = "SELECT * FROM journal";
            return _db.ExecuteQuery<Journal>(query).ToList();
        }

        public List<Journal> GetJournalsByUserId(int userId)
        {
            var query = "SELECT * FROM journal WHERE user_id = @UserId";
            _db.AddParameter("UserId", userId);
            return _db.ExecuteQuery<Journal>(query).ToList();
        }

        public List<Role> GetRoles()
        {
            var query = "SELECT * FROM roles";
            return _db.ExecuteQuery<Role>(query).ToList();
        }
    }
}