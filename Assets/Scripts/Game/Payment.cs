using System;
using System.Runtime.InteropServices;

[System.Serializable]
public struct Payment {

	public PaymentType PaymentType { get; private set; }
	public int PayBankAmount { get; private set; }
	public int PayWestAmount { get; private set; }
	public int PayEastAmount { get; private set; }
	public int TotalAmount {
		get {
			return PayBankAmount + PayWestAmount + PayEastAmount;
		}
	}

	public Payment(PaymentType paymentType, int payBankAmount, int payWestAmount, int payEastAmount) {
		PaymentType = paymentType;
		PayBankAmount = payBankAmount;
		PayWestAmount = payWestAmount;
		PayEastAmount = payEastAmount;
	}

	public override bool Equals(object obj) {
		Payment other = (Payment)obj;
		return PaymentType.Equals(other.PaymentType) && PayBankAmount.Equals(other.PayBankAmount) &&
			PayWestAmount.Equals(other.PayWestAmount) && PayEastAmount.Equals(other.PayEastAmount);
	}

	public override int GetHashCode() {
		int hash = 0;
		hash = hash * 23 + PaymentType.GetHashCode();
		hash = hash * 23 + PayBankAmount.GetHashCode();
		hash = hash * 23 + PayWestAmount.GetHashCode();
		hash = hash * 23 + PayEastAmount.GetHashCode();
		return hash;
	}

	public static byte[] Serialize(object paymentObject) {
		Payment payment = (Payment)paymentObject;
		
		int size = Marshal.SizeOf(payment);
		byte[] bytes = new byte[size];
		IntPtr ptr = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(payment, ptr, true);
		Marshal.Copy(ptr, bytes, 0, size);
		Marshal.FreeHGlobal(ptr);

		return bytes;
	}

	public static object Deserialize(byte[] paymentBytes) {
		Payment payment = new Payment();

		int size = Marshal.SizeOf(payment);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		Marshal.Copy(paymentBytes, 0, ptr, size);
		payment = (Payment)Marshal.PtrToStructure(ptr, payment.GetType());
		Marshal.FreeHGlobal(ptr);
		
		return payment;
	}

	public static Payment operator +(Payment p1, Payment p2) {
		Payment payment = new Payment();
		payment.PaymentType = PaymentType.Normal;
		payment.PayBankAmount = p1.PayBankAmount + p2.PayBankAmount;
		payment.PayWestAmount = p1.PayWestAmount + p2.PayWestAmount;
		payment.PayEastAmount = p1.PayEastAmount + p2.PayEastAmount;
		return payment;
	}

	public static Payment operator -(Payment p1, Payment p2) {
		Payment payment = new Payment();
		payment.PaymentType = PaymentType.Normal;
		payment.PayBankAmount = Math.Max(p1.PayBankAmount - p2.PayBankAmount, 0);
		payment.PayWestAmount = Math.Max(p1.PayWestAmount - p2.PayWestAmount, 0);
		payment.PayEastAmount = Math.Max(p1.PayEastAmount - p2.PayEastAmount, 0);
		return payment;
	}

	public static bool operator <(Payment p1, Payment p2) {
		return p1.TotalAmount < p2.TotalAmount;
	}

	public static bool operator >(Payment p1, Payment p2) {
		return p1.TotalAmount > p2.TotalAmount;
	}

}