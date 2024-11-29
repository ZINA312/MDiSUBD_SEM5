
using App.Entities;

namespace App.Services.DataService
{
    public interface IDataService
    {
        public bool CreateUser(User user);
        public bool UpdateUser(User user);
        public bool DeleteUser(User user);
        public bool CreateCart(Cart cart);
        public bool AddToCart(Cart cart, Product product, int quantity);
        public bool DeleteFromCart(Cart cart, Product product);
        public bool ChangeProductQuantity(Cart cart, Product product, int newQuantity);
        public bool AddCategory(Category category);
        public bool DeleteCategory(Category category);
        public bool AddReview(Review review);
        public bool DeleteReview(Review review);
        public bool AddOrder(Order order);
        public bool DeleteOrder(Order order);
        public bool UpdateOrder(Order order);
        public bool AddPaymentMethod(PaymentMethod paymentMethod);
        public bool DeletePaymentMethod(PaymentMethod paymentMethod);
        public bool AddDeliveryMethod(DeliveryMethod deliveryMethod);
        public bool DeleteDeliveryMethod(DeliveryMethod deliveryMethod);
        public bool AddRole(Role role);
        public bool DeleteRole(Role role);
        public bool AddProduct(Product product);
        public bool DeleteProduct(Product product);
        public bool UpdateProduct(Product product);
        public bool AddProductOrder(int product_id, int order_id, decimal total_price);
        public void ClearCart(int cartId);
        public Cart GetCartByUserId(int User_Id);
        public List<CartItem> GetProductsFromCart(int cartId);
        public Product GetProductById(int id);
        public List<Product> GetProducts(int? Category);
        public User GetUserById(int id);
        public User GetUserByEmail(string email);
        public List<Category> GetCategories();
        public List<Review> GetReviews(int Product_Id);
        public Order GetOrder(int user_id, DateTime date);
        public List<Order> GetOrders();
        public List<Order> GetOrdersByUserId(int User_Id);
        public List<PaymentMethod> GetPaymentMethods();
        public List<DeliveryMethod> GetDeliveryMethods();
        public List<Journal> GetJournals();
        public List<Journal> GetJournalsByUserId(int User_Id);
        public List<Role> GetRoles();
    }
}
