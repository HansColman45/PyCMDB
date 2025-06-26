package be.cmdb.model;

public class Identity {
    private int idenId;
    private String firstName;
    private String lastName;
    private String email;
    private String company;
    private String userId;

    public int getIdenId() {
        return idenId;
    }

    public void setIdenId(int idenId) {
        this.idenId = idenId;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getCompany() {
        return company;
    }

    public void setCompany(String company) {
        this.company = company;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }
}
