package be.cmdb.dao;

import be.cmdb.model.AssetType;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class AssetTypeDAO implements BaseDAO<AssetType, Integer>{

    @Override
    public AssetType save(Session session, AssetType entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public AssetType update(Session session, AssetType entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public AssetType findById(Session session, Integer id) {
        return session.find(AssetType.class, id);
    }

    @Override
    public List<AssetType> findAll(Session session) {
        Query<AssetType> query = session.createQuery("FROM AssetType", AssetType.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, AssetType entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        AssetType type = findById(session,id);
        if(type != null) {
            delete(session, type);
        } else {
            // Optionally handle the case where the asset type is not found
            System.out.println("AssetType with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> q = session.createQuery("SELECT COUNT(a) FROM AssetType a", Long.class);
        return q.getSingleResult();
    }

    /**
     * Finds AssetTypes by their name.
     * @param session the Hibernate session
     * @param adminId The ID of the admin who last modified the asset type
     * @return a list of AssetTypes with the specified name
     */
    public List<AssetType> findByLastModifiedAdminId(Session session, int adminId) {
        Query<AssetType> query = session.createQuery("FROM AssetType WHERE lastModifiedAdminId = :adminId", AssetType.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
