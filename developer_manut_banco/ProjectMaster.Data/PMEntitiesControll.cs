using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using ProjectMaster.Core;
using ProjectMaster.Data;
using System.Collections;
using System.Data.Common;

namespace ProjectMaster.Data
{

    public partial class PMEntities
    {

        partial void OnContextCreated()
        {
            this.SavingChanges += new EventHandler(OnSavingChanges);
        }

        public static void OnSavingChanges(object sender, EventArgs e)
        {
            PMEntities dbEntity = new PMEntities();

            string originalValue;
            string currentValue;
            bool propertyChanged = false;

            var stateManager = ((PMEntities)sender).ObjectStateManager;
            var insertedEntities = stateManager.GetObjectStateEntries(EntityState.Added);
            
            #region $ Audit Inserted Entities $

            foreach (ObjectStateEntry insertedEntryEntity in insertedEntities)
            {
                //This is for relationships that get modified  
                if (insertedEntryEntity.IsRelationship && insertedEntryEntity.EntitySet.Name != "pm_auditoria_log" && insertedEntryEntity.EntitySet.Name != "pm_auditoria_log_detalhe")
                {
                    EntityKey entityKeyFrom = (EntityKey)insertedEntryEntity.CurrentValues[1];
                    EntityKey entityKeyTo = (EntityKey)insertedEntryEntity.CurrentValues[0];

                    //Log the change 
                    //Create a audit header record 

                    pm_auditoria_log audit = new pm_auditoria_log();
                    audit.acao = "Update";
                    audit.data_acao = DateTime.Now;
                    audit.nome_tabela = entityKeyFrom.EntitySetName;

                    if (entityKeyFrom.EntityKeyValues != null)
                    {
                        audit.id_record = long.Parse(entityKeyFrom.EntityKeyValues[0].Value.ToString());
                    }

                    if (Context.UsuarioOnLine != null)
                    {
                        audit.id_usuario = Context.UsuarioOnLine.id_usuario;
                    }

                    //Create the Audit Detail Record 
                    //audit.pm_auditoria_log_detalhe = new TrackableCollection<pm_auditoria_log_detalhe>(); //new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();
                    audit.pm_auditoria_log_detalhe = new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();

                    //Log the change 
                    pm_auditoria_log_detalhe auditDetail = new pm_auditoria_log_detalhe();
                    auditDetail.nome_coluna = entityKeyTo.EntityKeyValues[0].Key.ToString();
                    auditDetail.valor_original = "";
                    auditDetail.valor_corrente = entityKeyTo.EntityKeyValues[0].Value.ToString();

                    dbEntity.pm_auditoria_log.AddObject(audit);

                }
                else //Not a relationship 
                {
                    //As long as the record being inserted isn't an auditlog then track changes 
                    if (insertedEntryEntity.EntitySet.Name != "pm_auditoria_log" && insertedEntryEntity.EntitySet.Name != "pm_auditoria_log_detalhe")
                    {
                        //Log the change 
                        //Create a audit header record 
                        pm_auditoria_log audit = new pm_auditoria_log();
                        audit.acao = "New";
                        audit.data_acao = DateTime.Now;
                        audit.nome_tabela = insertedEntryEntity.EntitySet.Name;

                        try
                        {
                            audit.id_record = long.Parse(insertedEntryEntity.CurrentValues[0].ToString());
                        }
                        catch { }

                        if (Context.UsuarioOnLine != null)
                        {
                            audit.id_usuario = Context.UsuarioOnLine.id_usuario;
                        }

                        //audit.pm_auditoria_log_detalhe = new TrackableCollection<pm_auditoria_log_detalhe>(); //new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();
                        audit.pm_auditoria_log_detalhe = new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();

                        //Go through all of the modified properties and add an audit detail record 
                        foreach (System.Data.Common.FieldMetadata fm in insertedEntryEntity.CurrentValues.DataRecordInfo.FieldMetadata)
                        {
                            var columnName = fm.FieldType.Name.ToString();
                            currentValue = insertedEntryEntity.CurrentValues[fm.Ordinal].ToString();

                            if (currentValue.Trim() != "")
                            {
                                //Log the change 
                                pm_auditoria_log_detalhe auditDetail = new pm_auditoria_log_detalhe();
                                auditDetail.nome_coluna = columnName;
                                auditDetail.valor_original = "";
                                auditDetail.valor_corrente = currentValue;

                                audit.pm_auditoria_log_detalhe.Add(auditDetail);
                            }
                        }

                        dbEntity.pm_auditoria_log.AddObject(audit);
                    }
                }
            }
            #endregion

            var modifiedEntities = stateManager.GetObjectStateEntries(EntityState.Modified);

            #region $ Audit Modified Entities $
            foreach (ObjectStateEntry modifiedEntryEntity in modifiedEntities)
            {
                //Check if any properties changed 
                foreach (string modifiedProperty in modifiedEntryEntity.GetModifiedProperties())
                {
                    originalValue = modifiedEntryEntity.OriginalValues[modifiedProperty].ToString();
                    currentValue = modifiedEntryEntity.CurrentValues[modifiedProperty].ToString();

                    if (originalValue.Trim() == currentValue.Trim())
                    {
                        continue;
                    }

                    propertyChanged = true;
                }

                //Only if a property has changed do we audit the change 
                if (propertyChanged)
                {
                    //Log the change 
                    //Create a audit header record 
                    pm_auditoria_log audit = new pm_auditoria_log();

                    audit.acao = "Update";
                    audit.data_acao = DateTime.Now;
                    audit.nome_tabela = modifiedEntryEntity.EntitySet.Name;

                    if (Context.UsuarioOnLine != null)
                    {
                        audit.id_usuario = Context.UsuarioOnLine.id_usuario;
                    }

                    audit.id_record = int.Parse(modifiedEntryEntity.EntityKey.EntityKeyValues[0].Value.ToString());
                    //audit.pm_auditoria_log_detalhe = new TrackableCollection<pm_auditoria_log_detalhe>(); //new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();
                    audit.pm_auditoria_log_detalhe = new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();

                    //Go through all of the modified properties and add an audit detail record 
                    foreach (string modifiedProperty in modifiedEntryEntity.GetModifiedProperties())
                    {
                        originalValue = modifiedEntryEntity.OriginalValues[modifiedProperty].ToString();
                        currentValue = modifiedEntryEntity.CurrentValues[modifiedProperty].ToString();

                        if (originalValue.Trim() != currentValue.Trim())
                        {
                            pm_auditoria_log_detalhe auditDetail = new pm_auditoria_log_detalhe();
                            auditDetail.nome_coluna = modifiedProperty;
                            auditDetail.valor_original = originalValue;
                            auditDetail.valor_corrente = currentValue;
                            audit.pm_auditoria_log_detalhe.Add(auditDetail);
                        }
                    }

                    dbEntity.pm_auditoria_log.AddObject(audit);
                }
            }

            #endregion

            var deletedEntities = stateManager.GetObjectStateEntries(EntityState.Deleted);

            #region $ Audit Deleted Entities $ 
            foreach (ObjectStateEntry deletedEntryEntity in deletedEntities)
            {
                //This is for relationships that get modified 
                if (!deletedEntryEntity.IsRelationship)
                {
                    //
                    //Not sure what to do here 

                    //Log the change 
                    //Create a audit header record 
                    pm_auditoria_log audit = new pm_auditoria_log();

                    audit.acao = "Delete";
                    audit.data_acao = DateTime.Now;
                    audit.nome_tabela = deletedEntryEntity.EntitySet.Name;

                    if (Context.UsuarioOnLine != null)
                    {
                        audit.id_usuario = Context.UsuarioOnLine.id_usuario;
                    }

                    audit.id_record = int.Parse(deletedEntryEntity.EntityKey.EntityKeyValues[0].Value.ToString());
                    //audit.pm_auditoria_log_detalhe = new TrackableCollection<pm_auditoria_log_detalhe>(); // new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();
                    audit.pm_auditoria_log_detalhe = new System.Data.Objects.DataClasses.EntityCollection<pm_auditoria_log_detalhe>();

                    //Go through all of the modified properties and add an audit detail record 
                    DbDataRecord modifiedProperty = deletedEntryEntity.OriginalValues;

                    for (int i = 0; i < modifiedProperty.FieldCount; i++)
                    {
                        originalValue = deletedEntryEntity.OriginalValues[modifiedProperty.GetName(i)].ToString();
                        currentValue = deletedEntryEntity.OriginalValues[modifiedProperty.GetName(i)].ToString();

                        pm_auditoria_log_detalhe auditDetail = new pm_auditoria_log_detalhe();
                        auditDetail.nome_coluna = modifiedProperty.GetName(i);
                        auditDetail.valor_original = originalValue;
                        auditDetail.valor_corrente = currentValue;
                        audit.pm_auditoria_log_detalhe.Add(auditDetail);
                    }

                    dbEntity.pm_auditoria_log.AddObject(audit);
                }
            }
            #endregion

            dbEntity.SavingChanges -= new EventHandler(OnSavingChanges);
            dbEntity.SaveChanges();
            dbEntity.SavingChanges += new EventHandler(OnSavingChanges);

        }
    }

}