using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BcsAdmin.DAL.Models
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<EpAnnotation> EpAnnotation { get; set; }
        public virtual DbSet<EpAttribute> EpAttribute { get; set; }
        public virtual DbSet<EpAttributeValue> EpAttributeValue { get; set; }
        public virtual DbSet<EpClassification> EpClassification { get; set; }
        public virtual DbSet<EpContent> EpContent { get; set; }
        public virtual DbSet<EpCountry> EpCountry { get; set; }
        public virtual DbSet<EpEntity> EpEntity { get; set; }
        public virtual DbSet<EpEntityClassification> EpEntityClassification { get; set; }
        public virtual DbSet<EpEntityComposition> EpEntityComposition { get; set; }
        public virtual DbSet<EpEntityCompositionInstance> EpEntityCompositionInstance { get; set; }
        public virtual DbSet<EpEntityLocation> EpEntityLocation { get; set; }
        public virtual DbSet<EpEntityNote> EpEntityNote { get; set; }
        public virtual DbSet<EpEntityOrganism> EpEntityOrganism { get; set; }
        public virtual DbSet<EpExperiment> EpExperiment { get; set; }
        public virtual DbSet<EpExperimentNote> EpExperimentNote { get; set; }
        public virtual DbSet<EpExperimentParameter> EpExperimentParameter { get; set; }
        public virtual DbSet<EpExperimentParameterItem> EpExperimentParameterItem { get; set; }
        public virtual DbSet<EpExperimentParameterValue> EpExperimentParameterValue { get; set; }
        public virtual DbSet<EpExperimentSeries> EpExperimentSeries { get; set; }
        public virtual DbSet<EpExperimentToEntity> EpExperimentToEntity { get; set; }
        public virtual DbSet<EpExperimentToModel> EpExperimentToModel { get; set; }
        public virtual DbSet<EpExperimentVariable> EpExperimentVariable { get; set; }
        public virtual DbSet<EpExperimentVariableToEntity> EpExperimentVariableToEntity { get; set; }
        public virtual DbSet<EpExperimentVariableToModelSpecie> EpExperimentVariableToModelSpecie { get; set; }
        public virtual DbSet<EpExperimentVariableValue> EpExperimentVariableValue { get; set; }
        public virtual DbSet<EpFunction> EpFunction { get; set; }
        public virtual DbSet<EpFunctionItem> EpFunctionItem { get; set; }
        public virtual DbSet<EpGallery> EpGallery { get; set; }
        public virtual DbSet<EpLanguages> EpLanguages { get; set; }
        public virtual DbSet<EpMailmsg> EpMailmsg { get; set; }
        public virtual DbSet<EpMenu> EpMenu { get; set; }
        public virtual DbSet<EpMenuHistory> EpMenuHistory { get; set; }
        public virtual DbSet<EpModel> EpModel { get; set; }
        public virtual DbSet<EpModelDataset> EpModelDataset { get; set; }
        public virtual DbSet<EpModelGraphset> EpModelGraphset { get; set; }
        public virtual DbSet<EpModelGraphsetItem> EpModelGraphsetItem { get; set; }
        public virtual DbSet<EpModelParameter> EpModelParameter { get; set; }
        public virtual DbSet<EpModelReaction> EpModelReaction { get; set; }
        public virtual DbSet<EpModelReactionItem> EpModelReactionItem { get; set; }
        public virtual DbSet<EpModelReactionItemToReactionItem> EpModelReactionItemToReactionItem { get; set; }
        public virtual DbSet<EpModelReactionToReaction> EpModelReactionToReaction { get; set; }
        public virtual DbSet<EpModelSpecie> EpModelSpecie { get; set; }
        public virtual DbSet<EpModelSpecieToEntity> EpModelSpecieToEntity { get; set; }
        public virtual DbSet<EpNumbersAttributes> EpNumbersAttributes { get; set; }
        public virtual DbSet<EpNumbersGroupMembers> EpNumbersGroupMembers { get; set; }
        public virtual DbSet<EpNumbersGroups> EpNumbersGroups { get; set; }
        public virtual DbSet<EpNumbersValues> EpNumbersValues { get; set; }
        public virtual DbSet<EpOrganism> EpOrganism { get; set; }
        public virtual DbSet<EpReaction> EpReaction { get; set; }
        public virtual DbSet<EpReactionClassification> EpReactionClassification { get; set; }
        public virtual DbSet<EpReactionEquationEntity> EpReactionEquationEntity { get; set; }
        public virtual DbSet<EpReactionEquationVariable> EpReactionEquationVariable { get; set; }
        public virtual DbSet<EpReactionItem> EpReactionItem { get; set; }
        public virtual DbSet<EpReactionItemComposition> EpReactionItemComposition { get; set; }
        public virtual DbSet<EpReactionItemCompositionInstance> EpReactionItemCompositionInstance { get; set; }
        public virtual DbSet<EpReactionItemSpecinstance> EpReactionItemSpecinstance { get; set; }
        public virtual DbSet<EpReactionNote> EpReactionNote { get; set; }
        public virtual DbSet<EpReactionOrganism> EpReactionOrganism { get; set; }
        public virtual DbSet<EpRedoxState> EpRedoxState { get; set; }
        public virtual DbSet<EpSetup> EpSetup { get; set; }
        public virtual DbSet<EpTranslations> EpTranslations { get; set; }
        public virtual DbSet<EpUnit> EpUnit { get; set; }
        public virtual DbSet<EpUser> EpUser { get; set; }
        public virtual DbSet<EpVisualisation> EpVisualisation { get; set; }
        public virtual DbSet<EpVisualisationItem> EpVisualisationItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnectionString.FullString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpAnnotation>(entity =>
            {
                entity.ToTable("ep_annotation");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Default)
                    .HasColumnName("default")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ItemType)
                    .HasColumnName("itemType")
                    .HasMaxLength(40);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.TermId)
                    .HasColumnName("termId")
                    .HasMaxLength(50);

                entity.Property(e => e.TermType)
                    .HasColumnName("termType")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpAttribute>(entity =>
            {
                entity.ToTable("ep_attribute");

                entity.HasIndex(e => e.Type)
                    .HasName("type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Datatype)
                    .HasColumnName("datatype")
                    .HasMaxLength(20);

                entity.Property(e => e.Default)
                    .HasColumnName("default")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(40);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Required)
                    .HasColumnName("required")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Style)
                    .HasColumnName("style")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EpAttributeValue>(entity =>
            {
                entity.ToTable("ep_attribute_value");

                entity.HasIndex(e => e.AttributeId)
                    .HasName("attributeId");

                entity.HasIndex(e => e.ItemId)
                    .HasName("itemId");

                entity.HasIndex(e => new { e.ItemId, e.AttributeId })
                    .HasName("attrItemId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttributeId)
                    .HasColumnName("attributeId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Short)
                    .HasColumnName("short")
                    .HasMaxLength(60);

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("text");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<EpClassification>(entity =>
            {
                entity.ToTable("ep_classification");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpContent>(entity =>
            {
                entity.ToTable("ep_content");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("text");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<EpCountry>(entity =>
            {
                entity.ToTable("ep_country");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.Continent)
                    .HasColumnName("continent")
                    .HasMaxLength(50);

                entity.Property(e => e.LanguageId)
                    .HasColumnName("languageId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.NameLocal)
                    .HasColumnName("nameLocal")
                    .HasMaxLength(100);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpEntity>(entity =>
            {
                entity.ToTable("ep_entity");

                entity.HasMany(e => e.Components).WithOne(co=> co.ComposedEntity);
                entity.HasMany(e => e.Locations).WithOne(l => l.Entity);
                entity.HasMany(e => e.Children).WithOne(e => e.Parent);
                entity.HasMany(e => e.Notes).WithOne(n=> n.Entity);
                entity.HasMany(e => e.Classifications).WithOne(c=> c.Entity);

                entity.HasIndex(e => e.ParentId)
                    .HasName("parentId");

                entity.HasIndex(e => e.Type)
                    .HasName("type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);

                entity.Property(e => e.HierarchyType)
                    .HasColumnName("hierarchy_type")
                    .HasColumnType("int(4)");

                entity.Property(e => e.VisualisationXml)
                    .HasColumnName("visualisationXml")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpEntityClassification>(entity =>
            {
                entity.ToTable("ep_entity_classification");

                entity.HasIndex(e => e.EntityId)
                    .HasName("entityId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassificationId)
                    .HasColumnName("classificationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpEntityComposition>(entity =>
            {
                entity.ToTable("ep_entity_composition");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildEntityId)
                    .HasColumnName("childEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentEntityId)
                    .HasColumnName("parentEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpEntityCompositionInstance>(entity =>
            {
                entity.ToTable("ep_entity_composition_instance");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompositionId)
                    .HasColumnName("compositionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpEntityLocation>(entity =>
            {
                entity.ToTable("ep_entity_location");

                entity.HasIndex(e => e.ChildEntityId)
                    .HasName("childEntityId");

                entity.HasIndex(e => e.ParentEntityId)
                    .HasName("parentEntityId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildEntityId)
                    .HasColumnName("childEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentEntityId)
                    .HasColumnName("parentEntityId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpEntityNote>(entity =>
            {
                entity.ToTable("ep_entity_note");

                entity.HasIndex(e => e.EntityId)
                    .HasName("entityId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("text");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpEntityOrganism>(entity =>
            {
                entity.ToTable("ep_entity_organism");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GeneGroup)
                    .HasColumnName("geneGroup")
                    .HasMaxLength(255);

                entity.Property(e => e.OrganismId)
                    .HasColumnName("organismId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperiment>(entity =>
            {
                entity.ToTable("ep_experiment");

                entity.HasIndex(e => e.ExperimentSeriesId)
                    .HasName("experimentSeriesId");

                entity.HasIndex(e => e.ParentId)
                    .HasName("parentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.ExperimentSeriesId)
                    .HasColumnName("experimentSeriesId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Protocol)
                    .HasColumnName("protocol")
                    .HasColumnType("text");

                entity.Property(e => e.Started)
                    .HasColumnName("started")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EpExperimentNote>(entity =>
            {
                entity.ToTable("ep_experiment_note");

                entity.HasIndex(e => e.ExperimentId)
                    .HasName("experimentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExperimentId)
                    .HasColumnName("experimentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("text");

                entity.Property(e => e.Time).HasColumnName("time");
            });

            modelBuilder.Entity<EpExperimentParameter>(entity =>
            {
                entity.ToTable("ep_experiment_parameter");

                entity.HasIndex(e => e.Code)
                    .HasName("code");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.DataType)
                    .HasColumnName("dataType")
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Required)
                    .HasColumnName("required")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentParameterItem>(entity =>
            {
                entity.ToTable("ep_experiment_parameter_item");

                entity.HasIndex(e => e.ExperimentParameterId)
                    .HasName("experimentParameterId");

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.ExperimentParameterId)
                    .HasColumnName("experimentParameterId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(70);
            });

            modelBuilder.Entity<EpExperimentParameterValue>(entity =>
            {
                entity.ToTable("ep_experiment_parameter_value");

                entity.HasIndex(e => e.ExperimentId)
                    .HasName("experimentId");

                entity.HasIndex(e => e.ExperimentParameterId)
                    .HasName("experimentParameterId");

                entity.HasIndex(e => e.ValueDate)
                    .HasName("valueDate");

                entity.HasIndex(e => e.ValueNum)
                    .HasName("valueNum");

                entity.HasIndex(e => e.ValueText)
                    .HasName("valueText");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExperimentId)
                    .HasColumnName("experimentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExperimentParameterId)
                    .HasColumnName("experimentParameterId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ValueDate)
                    .HasColumnName("valueDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValueNum).HasColumnName("valueNum");

                entity.Property(e => e.ValueText)
                    .HasColumnName("valueText")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<EpExperimentSeries>(entity =>
            {
                entity.ToTable("ep_experiment_series");

                entity.HasIndex(e => e.UserId)
                    .HasName("userId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("lastUpdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(80);

                entity.Property(e => e.OverallDesign)
                    .HasColumnName("overallDesign")
                    .HasColumnType("text");

                entity.Property(e => e.Summary)
                    .HasColumnName("summary")
                    .HasColumnType("text");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentToEntity>(entity =>
            {
                entity.ToTable("ep_experiment_to_entity");

                entity.HasIndex(e => e.EntityId)
                    .HasName("entityId");

                entity.HasIndex(e => e.ExperimentVariableId)
                    .HasName("experimentVariableId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExperimentVariableId)
                    .HasColumnName("experimentVariableId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentToModel>(entity =>
            {
                entity.ToTable("ep_experiment_to_model");

                entity.HasIndex(e => e.ExperimentId)
                    .HasName("experimentId");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExperimentId)
                    .HasColumnName("experimentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Validated)
                    .HasColumnName("validated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValidationUserId)
                    .HasColumnName("validationUserId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentVariable>(entity =>
            {
                entity.ToTable("ep_experiment_variable");

                entity.HasIndex(e => e.ExperimentId)
                    .HasName("experimentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(30);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.ExperimentId)
                    .HasColumnName("experimentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Formula)
                    .HasColumnName("formula")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Protocol)
                    .HasColumnName("protocol")
                    .HasColumnType("text");

                entity.Property(e => e.UnitId)
                    .HasColumnName("unitId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentVariableToEntity>(entity =>
            {
                entity.ToTable("ep_experiment_variable_to_entity");

                entity.HasIndex(e => e.EntityId)
                    .HasName("entityId");

                entity.HasIndex(e => e.ExperimentVariableId)
                    .HasName("experimentVariableId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExperimentVariableId)
                    .HasColumnName("experimentVariableId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentVariableToModelSpecie>(entity =>
            {
                entity.ToTable("ep_experiment_variable_to_model_specie");

                entity.HasIndex(e => e.ExperimentVariableId)
                    .HasName("experimentVariableId");

                entity.HasIndex(e => e.ModelSpecieId)
                    .HasName("modelSpecieId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExperimentVariableId)
                    .HasColumnName("experimentVariableId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelSpecieId)
                    .HasColumnName("modelSpecieId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpExperimentVariableValue>(entity =>
            {
                entity.ToTable("ep_experiment_variable_value");

                entity.HasIndex(e => e.ExperimentVariableId)
                    .HasName("experimentVariableId");

                entity.HasIndex(e => e.Time)
                    .HasName("time");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExperimentVariableId)
                    .HasColumnName("experimentVariableId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<EpFunction>(entity =>
            {
                entity.ToTable("ep_function");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Reaction)
                    .HasColumnName("reaction")
                    .HasColumnType("text");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpFunctionItem>(entity =>
            {
                entity.ToTable("ep_function_item");

                entity.HasIndex(e => e.FunctionId)
                    .HasName("functionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.FunctionId)
                    .HasColumnName("functionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Multiple)
                    .HasColumnName("multiple")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);

                entity.Property(e => e.UnitId)
                    .HasColumnName("unitId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpGallery>(entity =>
            {
                entity.ToTable("ep_gallery");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasMaxLength(255);

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .HasMaxLength(255);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpLanguages>(entity =>
            {
                entity.ToTable("ep_languages");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Abbr)
                    .IsRequired()
                    .HasColumnName("abbr")
                    .HasMaxLength(5);

                entity.Property(e => e.ActiveAdmin)
                    .HasColumnName("activeAdmin")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActiveWeb)
                    .HasColumnName("activeWeb")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpMailmsg>(entity =>
            {
                entity.ToTable("ep_mailmsg");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("text");

                entity.Property(e => e.LangId)
                    .HasColumnName("langId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpMenu>(entity =>
            {
                entity.ToTable("ep_menu");

                entity.HasIndex(e => e.LangId)
                    .HasName("langId");

                entity.HasIndex(e => e.Name)
                    .HasName("name");

                entity.HasIndex(e => e.NameUrl)
                    .HasName("nameUrl");

                entity.HasIndex(e => e.ParentId)
                    .HasName("parentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Auth)
                    .HasColumnName("auth")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Default)
                    .HasColumnName("default")
                    .HasColumnType("int(11)");

                entity.Property(e => e.H1Title)
                    .HasColumnName("h1Title")
                    .HasMaxLength(255);

                entity.Property(e => e.LangId)
                    .HasColumnName("langId")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MetaDesc)
                    .HasColumnName("metaDesc")
                    .HasMaxLength(255);

                entity.Property(e => e.MetaKeys)
                    .HasColumnName("metaKeys")
                    .HasMaxLength(255);

                entity.Property(e => e.MetaTitle)
                    .HasColumnName("metaTitle")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.NameUrl)
                    .HasColumnName("nameUrl")
                    .HasMaxLength(255);

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EpMenuHistory>(entity =>
            {
                entity.ToTable("ep_menu_history");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NameUrl)
                    .HasColumnName("nameUrl")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpModel>(entity =>
            {
                entity.ToTable("ep_model");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Solver)
                    .IsRequired()
                    .HasColumnName("solver")
                    .HasMaxLength(10);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(40);

                entity.Property(e => e.UnitId)
                    .HasColumnName("unitId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VisualisationXml)
                    .HasColumnName("visualisationXml")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpModelDataset>(entity =>
            {
                entity.ToTable("ep_model_dataset");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("text");

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Public)
                    .HasColumnName("public")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Solver)
                    .IsRequired()
                    .HasColumnName("solver")
                    .HasMaxLength(10);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpModelGraphset>(entity =>
            {
                entity.ToTable("ep_model_graphset");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpModelGraphsetItem>(entity =>
            {
                entity.ToTable("ep_model_graphset_item");

                entity.HasIndex(e => e.GraphsetId)
                    .HasName("graphsetId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.GraphsetId)
                    .HasColumnName("graphsetId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelSpecieId)
                    .HasColumnName("modelSpecieId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Side)
                    .HasColumnName("side")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpModelParameter>(entity =>
            {
                entity.ToTable("ep_model_parameter");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(40);

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Solver)
                    .IsRequired()
                    .HasColumnName("solver")
                    .HasMaxLength(10);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EpModelReaction>(entity =>
            {
                entity.ToTable("ep_model_reaction");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.FunctionId)
                    .HasColumnName("functionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EpModelReactionItem>(entity =>
            {
                entity.ToTable("ep_model_reaction_item");

                entity.HasIndex(e => e.ModelReactionId)
                    .HasName("modelReactionId");

                entity.HasIndex(e => e.ModelSpecieId)
                    .HasName("modelSpecieId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.FunctionItemId)
                    .HasColumnName("functionItemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsGlobal)
                    .HasColumnName("isGlobal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelReactionId)
                    .HasColumnName("modelReactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelSpecieId)
                    .HasColumnName("modelSpecieId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Stoichiometry)
                    .HasColumnName("stoichiometry")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<EpModelReactionItemToReactionItem>(entity =>
            {
                entity.ToTable("ep_model_reaction_item_to_reaction_item");

                entity.HasIndex(e => e.ModelReactionItemId)
                    .HasName("modelReactionItemId");

                entity.HasIndex(e => e.ReactionItemId)
                    .HasName("reactionItemId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ModelReactionItemId)
                    .HasColumnName("modelReactionItemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionItemId)
                    .HasColumnName("reactionItemId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpModelReactionToReaction>(entity =>
            {
                entity.ToTable("ep_model_reaction_to_reaction");

                entity.HasIndex(e => e.ModelReactionId)
                    .HasName("modelReactionId");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("reactionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ModelReactionId)
                    .HasColumnName("modelReactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpModelSpecie>(entity =>
            {
                entity.ToTable("ep_model_specie");

                entity.HasIndex(e => e.ModelId)
                    .HasName("modelId");

                entity.HasIndex(e => e.ParentId)
                    .HasName("parentId");

                entity.HasIndex(e => new { e.ModelId, e.ParentId })
                    .HasName("model_parentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.DynExpression)
                    .HasColumnName("dynExpression")
                    .HasColumnType("text");

                entity.Property(e => e.EquationType)
                    .HasColumnName("equationType")
                    .HasMaxLength(40);

                entity.Property(e => e.InitExpression)
                    .HasColumnName("initExpression")
                    .HasColumnType("text");

                entity.Property(e => e.ModelId)
                    .HasColumnName("modelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);

                entity.Property(e => e.UnitId)
                    .HasColumnName("unitId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpModelSpecieToEntity>(entity =>
            {
                entity.ToTable("ep_model_specie_to_entity");

                entity.HasIndex(e => e.EntityId)
                    .HasName("entityId");

                entity.HasIndex(e => e.ModelSpecieId)
                    .HasName("modelSpecieId");

                entity.HasIndex(e => new { e.LocationId, e.EntityId })
                    .HasName("location_entityId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModelSpecieId)
                    .HasColumnName("modelSpecieId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpNumbersAttributes>(entity =>
            {
                entity.ToTable("ep_numbers_attributes");

                entity.HasIndex(e => e.GroupId)
                    .HasName("group_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.BcsId)
                    .HasColumnName("bcs_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BcsType)
                    .HasColumnName("bcs_type")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Keywords)
                    .HasColumnName("keywords")
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.UnitId)
                    .HasColumnName("unit_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.EpNumbersAttributes)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ep_numbers_attributes_ibfk_1");
            });

            modelBuilder.Entity<EpNumbersGroupMembers>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("ep_numbers_group_members");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Group)
                    .WithOne(p => p.EpNumbersGroupMembers)
                    .HasForeignKey<EpNumbersGroupMembers>(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ep_numbers_group_members_ibfk_1");
            });

            modelBuilder.Entity<EpNumbersGroups>(entity =>
            {
                entity.ToTable("ep_numbers_groups");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.SupervisorId)
                    .HasColumnName("supervisor_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpNumbersValues>(entity =>
            {
                entity.ToTable("ep_numbers_values");

                entity.HasIndex(e => e.AttributeId)
                    .HasName("attribute_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttributeId)
                    .HasColumnName("attribute_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ErrorMargin)
                    .HasColumnName("error_margin")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExperimentId)
                    .HasColumnName("experiment_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("text");

                entity.Property(e => e.OrganismId)
                    .HasColumnName("organism_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UnitId)
                    .HasColumnName("unit_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ValueFrom).HasColumnName("value_from");

                entity.Property(e => e.ValueTo).HasColumnName("value_to");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.EpNumbersValues)
                    .HasForeignKey(d => d.AttributeId)
                    .HasConstraintName("ep_numbers_values_ibfk_1");
            });

            modelBuilder.Entity<EpOrganism>(entity =>
            {
                entity.ToTable("ep_organism");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EpReaction>(entity =>
            {
                entity.ToTable("ep_reaction");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Equation)
                    .HasColumnName("equation")
                    .HasColumnType("text");

                entity.Property(e => e.IsValid)
                    .HasColumnName("isValid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Modifier)
                    .HasColumnName("modifier")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.VisualisationXml)
                    .HasColumnName("visualisationXml")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpReactionClassification>(entity =>
            {
                entity.ToTable("ep_reaction_classification");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("reactionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassificationId)
                    .HasColumnName("classificationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpReactionEquationEntity>(entity =>
            {
                entity.ToTable("ep_reaction_equation_entity");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("reactionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.EndPos)
                    .HasColumnName("endPos")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Specification)
                    .IsRequired()
                    .HasColumnName("specification")
                    .HasMaxLength(20);

                entity.Property(e => e.StartPos)
                    .HasColumnName("startPos")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EpReactionEquationVariable>(entity =>
            {
                entity.ToTable("ep_reaction_equation_variable");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("reactionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EndPos)
                    .HasColumnName("endPos")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartPos)
                    .HasColumnName("startPos")
                    .HasColumnType("tinyint(4) unsigned");

                entity.Property(e => e.Variable)
                    .IsRequired()
                    .HasColumnName("variable")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<EpReactionItem>(entity =>
            {
                entity.ToTable("ep_reaction_item");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsComposition)
                    .HasColumnName("isComposition")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("locationId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SpecEntityId)
                    .HasColumnName("specEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Stoichiometry)
                    .HasColumnName("stoichiometry")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);

                entity.Property(e => e.VarValue)
                    .HasColumnName("varValue")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpReactionItemComposition>(entity =>
            {
                entity.ToTable("ep_reaction_item_composition");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildEntityId)
                    .HasColumnName("childEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentItemId)
                    .HasColumnName("parentItemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpReactionItemCompositionInstance>(entity =>
            {
                entity.ToTable("ep_reaction_item_composition_instance");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompositionId)
                    .HasColumnName("compositionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EntityId)
                    .HasColumnName("entityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpReactionItemSpecinstance>(entity =>
            {
                entity.ToTable("ep_reaction_item_specinstance");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildEntityId)
                    .HasColumnName("childEntityId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentItemId)
                    .HasColumnName("parentItemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<EpReactionNote>(entity =>
            {
                entity.ToTable("ep_reaction_note");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("reactionId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasColumnType("datetime");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("text");

                entity.Property(e => e.Updated)
                    .HasColumnName("updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpReactionOrganism>(entity =>
            {
                entity.ToTable("ep_reaction_organism");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.OrganismId)
                    .HasColumnName("organismId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReactionId)
                    .HasColumnName("reactionId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<EpRedoxState>(entity =>
            {
                entity.ToTable("ep_redox_state");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Mark)
                    .HasColumnName("mark")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EpSetup>(entity =>
            {
                entity.ToTable("ep_setup");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Default)
                    .HasColumnName("default")
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.LangId)
                    .HasColumnName("langId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpTranslations>(entity =>
            {
                entity.ToTable("ep_translations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.LangId)
                    .HasColumnName("langId")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Trans)
                    .HasColumnName("trans")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpUnit>(entity =>
            {
                entity.ToTable("ep_unit");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParentTree)
                    .HasColumnName("parentTree")
                    .HasMaxLength(50);

                entity.Property(e => e.Rate).HasColumnName("rate");
            });

            modelBuilder.Entity<EpUser>(entity =>
            {
                entity.ToTable("ep_user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Access)
                    .HasColumnName("access")
                    .HasMaxLength(255);

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.City2)
                    .HasColumnName("city2")
                    .HasMaxLength(50);

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(100);

                entity.Property(e => e.Country2Id)
                    .HasColumnName("country2Id")
                    .HasColumnType("int(30)");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasColumnType("int(30)");

                entity.Property(e => e.Dic)
                    .HasColumnName("dic")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(10);

                entity.Property(e => e.Icdph)
                    .HasColumnName("icdph")
                    .HasMaxLength(50);

                entity.Property(e => e.Ico)
                    .HasColumnName("ico")
                    .HasMaxLength(50);

                entity.Property(e => e.LanguageId)
                    .HasColumnName("languageId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("text");

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(70);

                entity.Property(e => e.Street2)
                    .HasColumnName("street2")
                    .HasMaxLength(70);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasMaxLength(20);

                entity.Property(e => e.Zip2)
                    .HasColumnName("zip2")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<EpVisualisation>(entity =>
            {
                entity.ToTable("ep_visualisation");

                entity.HasIndex(e => e.ParentId)
                    .HasName("parentId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VisualisationXml)
                    .HasColumnName("visualisationXml")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EpVisualisationItem>(entity =>
            {
                entity.ToTable("ep_visualisation_item");

                entity.HasIndex(e => e.VisualisationId)
                    .HasName("visualisationId");

                entity.HasIndex(e => new { e.ItemId, e.Type })
                    .HasName("itemId_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nonrecursive)
                    .HasColumnName("nonrecursive")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(40);

                entity.Property(e => e.VisualisationId)
                    .HasColumnName("visualisationId")
                    .HasColumnType("int(11)");
            });
        }
    }
}
