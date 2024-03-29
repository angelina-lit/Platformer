using UnityEngine;
using UnityEngine.UI;

public class ManagePerksWindow : AnimatedWindow
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private ItemWidget _price;
    [SerializeField] private Text _info;
    [SerializeField] private Transform _perksContainer;

    private PredefinedDataGroup<string, PerkWidget> _dataGroup;
    private readonly CompositeDisposable _trash = new CompositeDisposable();

	protected override void Start()
	{
		base.Start();

        _dataGroup = new PredefinedDataGroup<string, PerkWidget>(_perksContainer);
	}
}
