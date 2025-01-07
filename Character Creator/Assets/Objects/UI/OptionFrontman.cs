using UnityEngine;
using BodyPartSwap;

public class OptionFrontman : MonoBehaviour
{
    //  Events:
    private QueueRequest m_requestBroadcast = null;
    
    //  Cache:
    private OptionHandler[] m_options = null;

    public void Setup(QueueRequest _request)
    {
        //  Subscribe to the request broadcast. Whichever class is responsible for setting up this one, will receive the request.
        m_requestBroadcast += _request;

        //  Get all the options from under us in the hierarchy.
        m_options = GetComponentsInChildren<OptionHandler>();

        //  Run the setup of all the option handlers.
        for (int i = 0; i < m_options.Length; i++)
        {
            m_options[i].Setup(OnSwitchCalled);
        }
    }
    
    /// <summary>
    /// This function is called whenever on of the option handlers makes a call to switch a body part.
    /// This command is then broadcasted via the delegate in this class, hopefully reaching the model's swap handler.
    /// </summary>
    private SwapCallbackResponse OnSwitchCalled(Options _type, int _index)
    {
        return m_requestBroadcast.Invoke(_type, _index);
    }

    /// <summary>
    /// A delegate that represents a request to the swap handler. Requesting a part info with the option type, and index.
    /// </summary>
    public delegate SwapCallbackResponse QueueRequest(Options _type, int _index);
}