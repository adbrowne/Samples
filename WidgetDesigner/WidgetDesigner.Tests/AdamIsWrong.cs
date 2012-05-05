using System;
using NUnit.Framework;

namespace WidgetDesigner.Tests
{
    [TestFixture]
    public class AdamIsWrong
    {
         [Test]
        public void CustomersAreEqual()
         {
             var guid = Guid.NewGuid();
             var id1 = new CustomerId(guid);
             var id2 = new CustomerId(guid);
             var customer1 = new Customer(id1);
             var customer2 = new Customer(id2);

             Assert.That(customer1, Is.EqualTo(customer2));
         }
    }

    public class Customer
    {
        private readonly CustomerId _id;

        public Customer(CustomerId id)
        {
            _id = id;
        }

        public bool Equals(Customer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._id, _id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Customer)) return false;
            return Equals((Customer) obj);
        }

        public override int GetHashCode()
        {
            return (_id != null ? _id.GetHashCode() : 0);
        }
    }

    public class BaseId
    {
        private readonly Guid _guid;

        public BaseId(Guid guid)
        {
            _guid = guid;
        }

        public bool Equals(BaseId other)
        {
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._guid.Equals(_guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CustomerId)) return false;
            return Equals((BaseId) obj);
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
    }

    public class CustomerId : BaseId
    {
        public CustomerId(Guid guid) : base(guid)
        {
        }
    }
}