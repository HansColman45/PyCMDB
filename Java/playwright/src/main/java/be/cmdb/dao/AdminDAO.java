package be.cmdb.dao;

import be.cmdb.model.Admin;
import org.hibernate.Session;
import java.util.List;

public class AdminDAO implements BaseDAO<Admin,Integer> {
    @Override
    public Admin save(Session session, Admin entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Admin update(Session session, Admin entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Admin findById(Session session, Integer id) {
        return session.find(Admin.class, id);
    }

    @Override
    public List<Admin> findAll(Session session) {
        String hql = "FROM Admin";
        return session.createQuery(hql, Admin.class).getResultList();
    }

    @Override
    public void delete(Session session, Admin entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer integer) {
        Admin admin = findById(session, integer);
        if (admin != null) {
            delete(session, admin);
        } else {
            throw new IllegalArgumentException("Admin with id " + integer + " does not exist.");
        }
    }

    @Override
    public long count(Session session) {
        String hql = "SELECT COUNT(*) FROM Admin";
        return session.createQuery(hql, Long.class).getSingleResult();
    }
}
