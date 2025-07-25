package be.cmdb.dao;

import be.cmdb.model.Device;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
 * Data Access Object for Device entities.
 */
public class DeviceDAO implements BaseDAO<Device, String> {

    @Override
    public Device save(Session session, Device entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Device update(Session session, Device entity) {
        return session.merge(entity);
    }

    @Override
    public Device findById(Session session, String id) {
        return session.find(Device.class, id);
    }

    @Override
    public List<Device> findAll(Session session) {
        return session.createQuery("from asset", Device.class).list();
    }

    @Override
    public void delete(Session session, Device entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, String id) {
        Device device = findById(session, id);
        if (device != null) {
            delete(session, device);
        }
    }

    @Override
    public long count(Session session) {
        return session.createQuery("select count(d) from asset d", Long.class).uniqueResult();
    }

    /**
     * Finds devices by their type ID.
     * @param session the Hibernate session
     * @param typeId the ID of the device type
     * @return List of devices matching the type ID
     */
    public List<Device> findByTypeId(Session session, int typeId) {
        Query<Device> devices = session.createQuery("from assset where typeId = :typeId", Device.class);
        devices.setParameter("typeId", typeId);
        return devices.getResultList();
    }

    /**
     * Finds devices by their category ID.
     * @param session the Hibernate session
     * @param categoryId the ID of the device category
     * @return List of devices matching the category ID
     */
    public List<Device> findByCategiryId(Session session, int categoryId) {
        Query<Device> devices = session.createQuery("from asset where categoryId = :categoryId", Device.class);
        devices.setParameter("categoryId", categoryId);
        return devices.getResultList();
    }

    /**
     * Finds devices by the last modified admin ID.
     * @param session the Hibernate session
     * @param lastModifiedAdminId the ID of the admin who last modified the device
     * @return List of devices that were last modified by the specified admin
     */
    public List<Device> findByLastModifiedAdminId(Session session, int lastModifiedAdminId) {
        Query<Device> query = session.createQuery("from asset where lastModifiedAdminId = :lastModifiedAdminId", Device.class);
        query.setParameter("lastModifiedAdminId", lastModifiedAdminId);
        return query.getResultList();
    }
}
