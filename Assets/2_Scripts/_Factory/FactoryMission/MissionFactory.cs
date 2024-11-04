using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFactory : MonoBehaviour
{
    public void CreateMission(MissionData pMission, Vector3 pPosition)
    {
        StartCoroutine(CCreateMission(pMission, pPosition));
    }

    public IEnumerator CCreateMission(MissionData pMission, Vector3 pPosition)
    {
        switch (pMission.MissionType)
        {
            case MissionType.SURVIVE:
                {
                    GameObject MissionGo = new GameObject(pMission.MissionType.ToString());

                    MissionGo.AddComponent<SurviversMission>();

                    MissionGo.transform.position = pPosition;
                    MissionGo.transform.rotation = Quaternion.identity;
                    MissionGo.transform.localScale = Vector3.one;

                    if (MissionGo.GetComponent<SurviversMission>())
                    {
                        yield return new WaitUntil(() => MissionGo.GetComponent<SurviversMission>().SpriteRenderer);
                        MissionGo.GetComponent<SurviversMission>().MissionSetting(MissionGo.GetComponent<SurviversMission>(), pMission);

                        MissionGo.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        MissionGo.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        MissionGo.GetComponent<SurviversMission>().MissionType = pMission.MissionType;

                        AllManager.instance.GameModeM.missionList.Add(MissionGo.GetComponent<SurviversMission>());
                    }
                }
                break;
            case MissionType.DEFEND:
                {
                    GameObject MissionGo = new GameObject(pMission.MissionType.ToString());

                    MissionGo.AddComponent<DefendMission>();

                    MissionGo.transform.position = pPosition;
                    MissionGo.transform.rotation = Quaternion.identity;
                    MissionGo.transform.localScale = Vector3.one;

                    if (MissionGo.GetComponent<DefendMission>())
                    {
                        yield return new WaitUntil(() => MissionGo.GetComponent<DefendMission>().SpriteRenderer);
                        MissionGo.GetComponent<DefendMission>().MissionSetting(MissionGo.GetComponent<DefendMission>(), pMission);

                        MissionGo.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        MissionGo.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        MissionGo.GetComponent<DefendMission>().MissionType = pMission.MissionType;

                        AllManager.instance.GameModeM.missionList.Add(MissionGo.GetComponent<DefendMission>());
                    }
                }
                break;
            case MissionType.FOLLOW:
                {
                    GameObject MissionGo = new GameObject(pMission.MissionType.ToString());

                    MissionGo.AddComponent<FollowMission>();

                    MissionGo.transform.position = pPosition;
                    MissionGo.transform.rotation = Quaternion.identity;
                    MissionGo.transform.localScale = Vector3.one;

                    if (MissionGo.GetComponent<FollowMission>())
                    {
                        yield return new WaitUntil(() => MissionGo.GetComponent<FollowMission>().SpriteRenderer);
                        MissionGo.GetComponent<FollowMission>().MissionSetting(MissionGo.GetComponent<FollowMission>(), pMission);

                        MissionGo.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        MissionGo.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        MissionGo.GetComponent<FollowMission>().MissionType = pMission.MissionType;

                        AllManager.instance.GameModeM.missionList.Add(MissionGo.GetComponent<FollowMission>());
                    }
                }
                break;
            default:
                break;
        }

    }


}
