# Testing Notes

1. **Tests:** total 20, passing 20, failing 0.

2. **Line coverage:** 90.5% overall (Domain 100%, Application 95.8%).

3. **Tools:**
   - **Moq:** mocked IInventoryService, IOrderRepository and INotificationService to verify the result and the calls (Times.Once / Times.Never).
   - **AutoFixture:** generated a random Order, customized so Quantity > 0 and UnitPrice >= 0.
   - **Bogus:** generated a realistic customer name and email using a fixed seed (1234).

4. **Fairness (Examples B and C):** <جملتك>

5. **Bugs found:** <none>