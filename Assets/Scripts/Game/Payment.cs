public struct Payment {

	public int PayBankAmount { get; private set; }
	public int PayWestAmount { get; private set; }
	public int PayEastAmount { get; private set; }

	public Payment(int payBankAmount, int payWestAmount, int payEastAmount) {
		PayBankAmount = payBankAmount;
		PayWestAmount = payWestAmount;
		PayEastAmount = payEastAmount;
	}

}