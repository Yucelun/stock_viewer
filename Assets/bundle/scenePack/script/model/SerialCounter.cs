
public class SerialCounter {
    private int counter = 0;
    public int generateSerialNo()
    {
        this.counter++;
        return this.counter;
    }

    public void replace(int counter) {
        this.counter = counter;
    }
}