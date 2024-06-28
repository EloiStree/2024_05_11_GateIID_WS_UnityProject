using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class TextDebugIIDSentMono : MonoBehaviour
{

    public List<int> m_lastInteger;
    public int m_maxDebugValues = 10;

    public UnityEvent<string> m_onChanged;
    public string m_title = "Received:";

    public void NotifyIntegerTransaction(byte[] sentIID) {
        if (sentIID.Length == 4 || sentIID.Length==12)
        {
            int value = BitConverter.ToInt32(sentIID, 0);
            NotifyIntegerTransaction(value);
        }
        if (sentIID.Length == 16) { 
        
            int value = BitConverter.ToInt32(sentIID, 4);
            NotifyIntegerTransaction(value);
        }
    }
    public string m_format = "\t{0}\t";
    public void NotifyIntegerTransaction(int value) { 
    
        m_lastInteger.Insert(0,value);

        while (m_lastInteger.Count>m_maxDebugValues)
        {
            m_lastInteger.RemoveAt( m_lastInteger.Count-1);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(m_title);
        foreach (var item in m_lastInteger)
        {
            byte[ ] bytes= BitConverter.GetBytes(item);
            sb.AppendLine(string.Format(m_format, item));
        }
        
        m_onChanged.Invoke(sb.ToString());
        
    }


}
