using QuodLib.DataModel.CustomAttributes;

namespace QuodLib.DataModel {
    public interface IRecord {
        int? ID { get; set; }

        [DbIgnore]
        bool IsNew
            => ID == null;
    }
}