using System;
using System.Runtime.InteropServices;

public struct Payment {

	public int PayBankAmount { get; private set; }
	public int PayWestAmount { get; private set; }
	public int PayEastAmount { get; private set; }

	public Payment(int payBankAmount, int payWestAmount, int payEastAmount) {
		PayBankAmount = payBankAmount;
		PayWestAmount = payWestAmount;
		PayEastAmount = payEastAmount;
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

}