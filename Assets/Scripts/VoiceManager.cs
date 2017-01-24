using UnityEngine;
using System.Collections;

public class VoiceManager : Singleton<VoiceManager> {

    public const string SELECT_MENU = "select_menu";
    public const string YOU_SELECT_AR_MODE = "selected_ar_menu";

    public const string SEARCH_DTC_FOR_YOUR_VEHICLE = "search_DTC_for_your_vehicle";
    public const string BEFORE_STARTING_DTC_SEARCE = "before_starting_DTC_Search";

    public const string NOW_SEARCHING_DTC = "now_searching_DTC";
    public const string PLEASE_WAIT_FOR_SECONDS = "please_wait_for_seconds";

    public const string Here_your_DTC_Result = "Here_your_DTC_Result";
    public const string Please_select_the_number_that_you_want_to_see_AR_repair_procedures = "Please_select_the_number_that_you_want_to_see_AR_repair_procedures";

    public const string Be_sure_to_shut_off_the_high_voltage_system_before_doing_any_work = "Be_sure_to_shut_off_the_high_voltage_system_before_doing_any_work";
    public const string Failure_to_follow_the_safety_instructions_may_result_in_serious_electrical_injuries = "Failure_to_follow_the_safety_instructions_may_result_in_serious_electrical_injuries";

    public const string Before_start_drag_the_3D_model_onto_the_High_Voltage_Joint_Box = "Before_start_drag_the_3D_model_onto_the_High_Voltage_Joint_Box";
    public const string If_done_say_OK = "If_done_say_OK";

    public const string Now_you_ll_start_the_high_voltage_joint_box_removal_procedures = "Now_you_ll_start_the_high_voltage_joint_box_removal_procedures";
    public const string Please_say_High_voltage_joint_box_or_touch_the_blinking_component = "Please_say_High_voltage_joint_box_or_touch_the_blinking_component";

    public const string Step_1 = "Step_1";
    public const string Disconnet_the_quick_charge_high_voltage_cable = "Disconnet_the_quick_charge_high_voltage_cable";
    public const string Disconnect_the_high_voltage_battery_cable = "Disconnect_the_high_voltage_battery_cable";

    public const string Loosen_four_bolts = "Loosen_four_bolts";
    public const string If_finished_say_Next = "If_finished_say_Next";

    public const string This_is_the_last_step = "This_is_the_last_step";
    public const string Remove_the_high_voltage_joint_box = "Remove_the_high_voltage_joint_box";

    public const string the_removal_procedure_finished = "the_removal_procedure_finished";
    public const string select_the_next_step_and_say_the_number = "select_the_next_step_and_say_the_number";

    public const string Drag_the_3D_model_onto_the_High_Voltage_Joint_Box__If_done_say_OK = "Drag_the_3D_model_onto_the_High_Voltage_Joint_Box__If_done_say_OK";
    public AudioSource source;
	void Start () {
        source = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Say(string filename)
    {
        source.clip = Resources.Load<AudioClip>("voice/"+ filename) ;
        source.Play();
    }

    public void Say(string filename, float time)
    {
        StartCoroutine(SayInTime(filename, time));
    }

    private IEnumerator SayInTime(string filename,float time)
    {
        yield return new WaitForSeconds(time);
        Say(filename);
    }

    public void CancelAll()
    {
        if (source != null)
        {
            source.Stop();
        }
        
        StopAllCoroutines();
    }
    
}
