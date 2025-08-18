package be.cmdb.helpers;

import be.cmdb.builders.LaptopBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.CategoryDAO;
import be.cmdb.dao.DeviceDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Admin;
import be.cmdb.model.Category;
import be.cmdb.model.Device;
import be.cmdb.model.Log;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class LaptopHelper {
    private static Transaction transaction;
    private static DeviceDAO deviceDAO;

    /**
     * Creates a basic Laptop with default values for testing purposes.
     * The laptop will be saved in the database and returned.
     *
     * @param session the Hibernate session to use
     * @param admin the Admin who last modified the laptop
     * @param active whether the laptop is active or not
     * @return a new Laptop entity persisted in the database
     */
    public static Device createBasicLaptop(Session session, Admin admin, boolean active){
        Category category = new CategoryDAO().findByCategory(session, "Laptop").get(0);

        transaction = session.beginTransaction();
        Device laptop = new LaptopBuilder()
            .withLastModifiedAdminId(admin.getAdminId())
            .withAssetCategoryId(category.getId())
            .build();
        deviceDAO = new DeviceDAO();
        laptop = deviceDAO.save(session, laptop);

        Log log = new LogBuilder()
            .withDevice(laptop)
            .withLogText("The Laptop with type  is created by Automation in table laptop")
            .build();

        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);

        transaction.commit();
        return laptop;
    }

    /**
     * Deletes a Laptop from the database along with its associated logs.
     * This method will also delete all logs related to the laptop's asset tag.
     *
     * @param session the Hibernate session to use
     * @param laptop the Laptop device to delete
     */
    public static void deleteLaptop(Session session, Device laptop){
        transaction = session.beginTransaction();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByAssetTag(session, laptop.getAssetTag());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        deviceDAO = new DeviceDAO();
        deviceDAO.delete(session, laptop);
        transaction.commit();
    }
}
