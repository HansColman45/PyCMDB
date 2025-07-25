package be.cmdb.dao;

import be.cmdb.model.Permission;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class PermissionDAO implements BaseDAO<Permission, Integer> {
    @Override
    public Permission save(Session session, Permission entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Permission update(Session session, Permission entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Permission findById(Session session, Integer id) {
        return session.find(Permission.class, id);
    }

    @Override
    public List<Permission> findAll(Session session) {
        Query<Permission> query = session.createQuery("FROM Permission", Permission.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Permission entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Permission permission = findById(session, id);
        if(permission != null) {
            delete(session, permission);
        } else {
            // Optionally handle the case where the mobile is not found
            System.out.println("Mobile with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(p) FROM Permission p", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds permissions by the last modified admin ID.
     * @param session the Hibernate session
     * @param adminId the ID of the admin who last modified the permission
     * @return a list of permissions modified by the specified admin
     */
    public List<Permission> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<Permission> query = session.createQuery("FROM Permission p WHERE p.lastModifiedAdmin.id = :adminId", Permission.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
