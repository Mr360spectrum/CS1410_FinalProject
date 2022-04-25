namespace lib;

/// <summary>
/// The interface that is implemented by inventory items that can be renamed.
/// </summary>
public interface IRenameable
{
    void Rename(string inRename);
}