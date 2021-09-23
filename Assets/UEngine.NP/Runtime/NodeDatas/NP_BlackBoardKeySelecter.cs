using System.Collections.Generic;
using Sirenix.OdinInspector;
using UEngine.NP;

public class NP_BlackBoardKeySelecter<T> where T : ANP_BBValue
{
    [LabelText("同步指定的黑板key")] [ValueDropdown("GetBBKeys")]
    public string BBKey;

#if UNITY_EDITOR
    private IEnumerable<string> GetBBKeys()
    {
        List<string> keys = new List<string>();
        if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
        {
            foreach (var bbValue in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
            {
                if (bbValue.Value.GetType() == typeof(T))
                {
                    keys.Add(bbValue.Key);
                }
            }

            return keys;
        }

        return null;
    }
#endif
}