using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    public GameObject player;
    private PlayerControl playerControlScript;
    private bool isExplain;

    public enum TutorialStep{
        MoveTutorial,
        BedTutorial,
        GamingTutorial,
        SojuTutorial,
        CoffeeTutorial,
        EnergyDrinkTutorial,
        EndingTutorial
    }

    private List<string> MoveTutorialMsg;
    private List<string> BedTutorialMsg;
    private List<string> GamingTutorialMsg;
    private List<string> SojuTutorialMsg;
    private List<string> CoffeeTutorialMsg;
    private List<string> EnergyDrinkTutorialMsg;
    private List<string> EndingTutorialMsg;
    public TextMeshProUGUI msg, guideMsg, explainMsg;
    public GameObject msgBox, energyDrink;
    private TutorialStep currentStep;
    private int currentIdx;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("No Player object in this map");
            }
        }

        playerControlScript = player?.GetComponent<PlayerControl>();
        if (playerControlScript == null) Debug.LogError("No PlayerControl script");
        
        isExplain = false;
        msg.text = "";
        guideMsg.text = "";
        explainMsg.text = "";
        msgBox.SetActive(false);

        MoveTutorialMsg = new List<string>();
        BedTutorialMsg = new List<string>();
        GamingTutorialMsg = new List<string>();
        SojuTutorialMsg = new List<string>();
        CoffeeTutorialMsg = new List<string>();
        EnergyDrinkTutorialMsg = new List<string>();
        EndingTutorialMsg = new List<string>();

        MoveTutorialMsg.Add("안녕하세요!\n\n주인공의 <color=red>악몽</color>속에 오신 것을 환영합니다..");
        MoveTutorialMsg.Add("지금부터 여러분은 \n불쌍한 주인공을 도와,\n\n목적지에 <color=yellow>과제</color>를 제출해서 <color=red>악몽</color>에서 \n깨어나도록 해줄 것입니다.");
        MoveTutorialMsg.Add("우선, 주인공을 움직이게 해봅시다.\n\n키보드 <color=red>방향키</color>를 눌러 \n주인공을 움직이게 할 수 있습니다.");

        BedTutorialMsg.Add("좋습니다!");
        BedTutorialMsg.Add("악몽 속에는 과제 제출을 막는 \n다양한 요소들이 있습니다.\n\n우선 <color=red>침대</color>에 대해 알아봅시다.");
        BedTutorialMsg.Add("주인공이 침대에 닿으면 졸게 되기 때문에,\n\n<color=red>3초</color>간 <color=red>정지</color>하게 됩니다.");
        BedTutorialMsg.Add("<color=red>침대</color>를 피해 이동해봅시다.");

        GamingTutorialMsg.Add("좋습니다!");
        GamingTutorialMsg.Add("이제 <color=orange>게임기</color>에 대해 알아봅시다.");
        GamingTutorialMsg.Add("주인공이 <color=orange>게임기</color>에 닿으면 딴짓을 하기 때문에,\n\n<color=red>5초</color>간 최대 속도가 <color=red>절반</color>이 됩니다.");
        GamingTutorialMsg.Add("<color=orange>게임기</color>를 피해 이동해봅시다.");

        SojuTutorialMsg.Add("좋습니다!");
        SojuTutorialMsg.Add("이번엔 <color=yellow>술</color>에 대해 알아봅시다.");
        SojuTutorialMsg.Add("주인공이 <color=yellow>술</color>을 마시게 되면,\n\n<color=red>5초</color>간 <color=red>좌우</color> 입력이 바뀌게 됩니다.");
        SojuTutorialMsg.Add("<color=yellow>술</color>을 피해 이동해봅시다.");

        CoffeeTutorialMsg.Add("좋습니다!");
        CoffeeTutorialMsg.Add("과제를 할 때, 항상 방해요소만 있는 것은 아닙니다.\n\n이번에는 도움을 주는 요소를 알아봅시다.");
        CoffeeTutorialMsg.Add("주인공이 <color=green>커피</color>를 마시게 되면,\n\n<color=red>5초</color>간 최대 속도가 <color=red>증가</color>합니다.");
        CoffeeTutorialMsg.Add("<color=green>커피</color>를 활용해 이동해봅시다.");

        EnergyDrinkTutorialMsg.Add("좋습니다!");
        EnergyDrinkTutorialMsg.Add("마지막으로, <color=#00FFFF>에너지 드링크</color>에 대해 알아봅시다.");
        EnergyDrinkTutorialMsg.Add("주인공이 <color=#00FFFF>에너지 드링크</color>를 마시게 되면,\n\n<color=red>3초</color>간 모든 방해 요소를 <color=red>무시</color>합니다.");
        EnergyDrinkTutorialMsg.Add("<color=#00FFFF>에너지 드링크</color>의 경우 처음부터 주어지고,\n\n<color=red>스페이스 바</color>를 눌러 사용할 수 있습니다.");
        EnergyDrinkTutorialMsg.Add("<color=#00FFFF>에너지 드링크</color>를 활용해 장애물을 돌파해 봅시다.");

        EndingTutorialMsg.Add("좋습니다!");
        EndingTutorialMsg.Add("이제 교수님께 첫번째 과제를 제출하고 악몽에서 탈출해요!!");
        EndingTutorialMsg.Add("앗 과제가 하나가 아니구나..\n\n이러면 다른 과제들도 해야 탈출할 수 있을 것 같네요..");
        EndingTutorialMsg.Add("도와주실거죠..?");

        StartStep(TutorialStep.MoveTutorial);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            NextMsg();
        }
    }

    private void NextMsg(){
        if(isExplain && !InGameUIControl.isMenuPopped){
            Debug.Log("What");
            currentIdx++;
            UpdateMsg();
        }
    }

    public void StartStep(TutorialStep step)
    {
        isExplain = true;
        currentStep = step;
        currentIdx = 0;
        msgBox.SetActive(true);
        playerControlScript.isExplain = true;
        Time.timeScale = 0f;
        UpdateMsg();
    }

    private void EndStep(){
        msgBox.SetActive(false);
        playerControlScript.isExplain = false;
        Time.timeScale = 1f;
        msg.text = "";
        guideMsg.text = "";
    }

    private void UpdateMsg(){
        guideMsg.text = "<color=yellow>Enter</color> 키로 계속";
        explainMsg.text = "";
        switch (currentStep){
            case TutorialStep.MoveTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= MoveTutorialMsg.Count){
                    explainMsg.text = "방향키를 눌러 이동해 보세요";
                    EndStep();
                }
                else msg.text = MoveTutorialMsg[currentIdx];
                break;
            case TutorialStep.BedTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= BedTutorialMsg.Count){
                    explainMsg.text = "침대에 닿으면 3초간 정지합니다";
                    EndStep();
                }
                else msg.text = BedTutorialMsg[currentIdx];
                break;
            case TutorialStep.GamingTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= GamingTutorialMsg.Count){
                    explainMsg.text = "게임기에 닿으면 5초간 느려집니다";
                    EndStep();
                }
                else msg.text = GamingTutorialMsg[currentIdx];
                break;
            case TutorialStep.SojuTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= SojuTutorialMsg.Count){
                    explainMsg.text = "술을 먹으면 5초간 좌우 방향키가 바뀝니다";
                    EndStep();
                }
                else msg.text = SojuTutorialMsg[currentIdx];
                break;
            case TutorialStep.CoffeeTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= CoffeeTutorialMsg.Count){
                    explainMsg.text = "커피를 먹으면 5초간 빨라집니다\n 속도, 가속도, 좌우 이동 속도 모두 빨라져요!";
                    EndStep();
                }
                else msg.text = CoffeeTutorialMsg[currentIdx];
                break;
            case TutorialStep.EnergyDrinkTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= EnergyDrinkTutorialMsg.Count){
                    explainMsg.text = "스페이스 바를 눌러 에너지 드링크 사용! \n 기존 장애물 효과를 무효로 하고 3초간 장애물을 무시해요!";
                    energyDrink.SetActive(true);
                    EndStep();
                }
                else msg.text = EnergyDrinkTutorialMsg[currentIdx];
                break;
            case TutorialStep.EndingTutorial:
                Debug.Assert(0 <= currentIdx);
                if(currentIdx >= EndingTutorialMsg.Count){
                    explainMsg.text = "";
                    EndStep();
                }
                else msg.text = EndingTutorialMsg[currentIdx];
                break;
        }
    }
}
