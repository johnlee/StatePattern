using System;

namespace DnetState
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("State Design Pattern");

            BookingContext e1 = new BookingContext();
            e1.SubmitDetails("John Smith", 1);
            e1.TransitionToState(new BookedState());
            e1.DatePassed();
            //STATE: New
            //STATE: Pending
            //STATE: Booked
            //Closing - Existing Booking Completed
            //STATE: Closed

            BookingContext e2 = new BookingContext();
            e2.SubmitDetails("John Doe", 3);
            e2.TransitionToState(new BookedState());
            e2.Cancel();
            //STATE: New
            //STATE: Pending
            //STATE: Booked
            //Closing - Existing Booking Canceled
            //STATE: Closed
        }

    }

    public class BookingContext
    {
        public string Attendee { get; set; }
        public int TicketCount { get; set; }
        public int BookingId { get; set; }
        private BookingState currentState { get; set; }

        public BookingContext()
        {
            TransitionToState(new NewState());
        }
        public void SubmitDetails(string attendee, int ticketCount)
        {
            currentState.EnterDetails(this, attendee, ticketCount);
        }
        public void Cancel()
        {
            currentState.Cancel(this);
        }
        public void DatePassed()
        {
            currentState.DatePassed(this);
        }
        public void TransitionToState(BookingState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }
        public void ShowState(string state)
        {
            Console.WriteLine($"STATE: {state}");
        }
    }

    public abstract class BookingState
    {
        public abstract void EnterState(BookingContext booking);
        public abstract void Cancel(BookingContext booking);
        public abstract void DatePassed(BookingContext booking);
        public abstract void EnterDetails(BookingContext booking, string attendee, int ticketCount);
    }

    public class NewState : BookingState
    {
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("New Booking Canceled"));
        }
        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("New Booking Expired"));
        }
        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            booking.Attendee = attendee;
            booking.TicketCount = ticketCount;
            booking.TransitionToState(new PendingState()); // Change state
        }
        public override void EnterState(BookingContext booking)
        {
            booking.BookingId = new Random().Next();
            booking.ShowState("New");
        }
    }

    public class ClosedState : BookingState
    {
        private string reason;
        public ClosedState(string reason)
        {
            this.reason = reason;
        }
        public override void Cancel(BookingContext booking)
        {
            throw new NotImplementedException();
        }
        public override void DatePassed(BookingContext booking)
        {
            throw new NotImplementedException();
        }
        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            throw new NotImplementedException();
        }
        public override void EnterState(BookingContext booking)
        {
            Console.WriteLine($"Closing - {reason}");
            booking.ShowState("Closed");
        }
    }

    public class PendingState : BookingState
    {
        public override void Cancel(BookingContext booking)
        {
            throw new NotImplementedException();
        }
        public override void DatePassed(BookingContext booking)
        {
            throw new NotImplementedException();
        }
        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            throw new NotImplementedException();
        }
        public override void EnterState(BookingContext booking)
        {
            booking.ShowState("Pending");
        }
    }

    public class BookedState : BookingState
    {
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Existing Booking Canceled"));
        }
        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Existing Booking Completed"));
        }
        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            throw new NotImplementedException();
        }
        public override void EnterState(BookingContext booking)
        {
            booking.ShowState("Booked");
        }
    }
}
